using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using marijnz.EditorCoroutines;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEditor;
using UnityEngine;

public class MoPubSDKManager : EditorWindow
{
    private const string downloadDir = "Assets/MoPub";
    private const string manifestURL = "https://mopub-mediation.firebaseio.com/.json";
    private const string migrateNote = "A legacy directory structure of MoPub was found in your project.\n" +
                                       "Pressing 'Migrate' will move any files in the legacy locations into the new " +
                                       "location, so all MoPub-related files are under Assets/MoPub/. Please open " +
                                       "the link below for details.\nBE AWARE THAT THE MIGRATION IS NOT REVERSIBLE.";
    private const string migrateLink = "https://developers.mopub.com/docs/unity/getting-started/#migrating-to-54";

    // Version and download info for the SDK.
    private string current;
    private string latest;
    private string packageUrl;
    private string filename;

    // Async download operations tracked here.
    private EditorCoroutines.EditorCoroutine coroutine;
    private WebClient downloader;
    private float progress;
    private string activity;
    private bool legacyMoPub;

    // For overriding button enable/disable states.
    private bool testing = false;

    // Custom style for LabelFields.
    private GUIStyle labelStyle;
    private GUIStyle labelStyleArea;
    private GUIStyle labelStyleLink;

    public static void ShowSDKManager()
    {
        GetWindow(typeof(MoPubSDKManager), true, "MoPub SDK Manager", true).Show();
    }

    void Awake()
    {
        labelStyle = new GUIStyle(EditorStyles.label) {
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };
        labelStyleArea = new GUIStyle(EditorStyles.label)
        {
            wordWrap = true
        };
        labelStyleLink = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = Color.blue },
            active = { textColor = Color.white },
        };


    }

    void OnEnable()
    {
        legacyMoPub = MoPubUpgradeMigration.LegacyMoPubPresent();
        coroutine = this.StartCoroutine(GetSDKVersions());
    }

    private void OnDisable()
    {
        CancelOperation();
    }

    public IEnumerator GetSDKVersions()
    {
        // Download the manifest from MoPub's status website.
        current = MoPub.moPubSDKVersion;
        activity = "Downloading SDK version manifest...";
        var www = new WWW(manifestURL);
        yield return www;

        // Got the file.  Now extract info on latest SDK available.
        var json = www.text;
        var dict = Json.Deserialize(json) as Dictionary<string,object>;
        object obj;
        if (dict != null && dict.TryGetValue("mopubBaseConfig", out obj))
            dict = obj as Dictionary<string,object>;
        if (dict != null && dict.TryGetValue("Unity", out obj))
            dict = obj as Dictionary<string, object>;
        if (dict != null) {
            if (dict.TryGetValue("version", out obj))
                latest = obj as string;
            if (dict.TryGetValue("filename", out obj))
                filename = obj as string;
            if (dict.TryGetValue("download_url", out obj))
                packageUrl = obj as string;
        }

        // Clear up the async-job state.
        coroutine = null;
        Repaint();
    }

    void OnGUI()
    {
        // New SDK to install?
        var canInstall = latest != null && (current == null || Version.Compare(current, latest) > 0);
        // Legacy MoPub directory structure found and no update available
        var canMigrate = legacyMoPub && !canInstall;
        // Is any async job in progress?
        var stillWorking = coroutine != null || downloader != null;

        GUILayoutOption buttonWidth = GUILayout.Width(60);

        GUILayout.Space(5);
        EditorGUILayout.LabelField("MoPub SDK", labelStyle, GUILayout.Height(20));

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        GUILayout.Space(10);

        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField("Current:", buttonWidth);
        EditorGUILayout.SelectableLabel(current ?? "--", buttonWidth);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField("Latest:", buttonWidth);
        EditorGUILayout.SelectableLabel(latest ?? "--", buttonWidth);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));
        EditorGUILayout.Space();
        // Disable the following button when an async operation is in progress, or there is no action needed
        // (i.e. nothing to install or clean up).
        GUI.enabled = !stillWorking && (canInstall || legacyMoPub) || testing;
        if (!canMigrate) {  // This is the default button to show.
            if (GUILayout.Button(current != null ? "Upgrade" : "Install", buttonWidth))
                this.StartCoroutine(DownloadSDK());
        } else {
            if (GUILayout.Button(new GUIContent("Migrate"), buttonWidth))
                LegacyCleanup();
        }
        GUI.enabled = true;
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        if (canMigrate) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);
            EditorGUILayout.LabelField(migrateNote, labelStyleArea, GUILayout.Height(70));
            if (GUILayout.Button(migrateLink, labelStyleLink))
            {
                Application.OpenURL("https://developers.mopub.com/docs/unity/getting-started/#migrating-to-54");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        // Indicate async operation in progress.
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField(stillWorking ? activity : " ");
        EditorGUILayout.EndHorizontal();
        if (stillWorking && progress > 0 &&
                EditorUtility.DisplayCancelableProgressBar("MoPub SDK Manager", activity, progress) &&
                Event.current.rawType == EventType.Repaint)  // OnGUI gets called several times per frame...
            CancelOperation();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        if (!stillWorking) {
            if (GUILayout.Button("Done", GUILayout.ExpandWidth(false)))
                Close();
        } else {
            if (GUILayout.Button("Cancel", buttonWidth))
                CancelOperation();
        }
        EditorGUILayout.EndHorizontal();

#if mopub_developer
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        testing = EditorGUILayout.ToggleLeft("Testing Mode", testing);
        EditorGUILayout.EndHorizontal();
#endif
    }

    private void CancelOperation()
    {
        // Stop any async action taking place.

        if (downloader != null) {
            downloader.CancelAsync(); // The coroutine should resume and clean up.
            return;
        }

        if (coroutine != null)
            this.StopCoroutine(coroutine.routine);
        if (progress > 0)
            EditorUtility.ClearProgressBar();
        coroutine = null;
        downloader = null;
        progress = 0;
    }

    private void LegacyCleanup()
    {
        MoPubUpgradeMigration.DoMigration();
        legacyMoPub = MoPubUpgradeMigration.LegacyMoPubPresent();
    }

    private IEnumerator DownloadSDK()
    {
        // Track download progress (updated by event callbacks below).
        bool ended = false;
        bool cancelled = false;
        Exception error = null;
        int oldPercentage = 0, newPercentage = 0;

        string path = Path.Combine(downloadDir, filename);

        Debug.LogFormat("Downloading {0} to {1}", packageUrl, path);
        activity = string.Format("Downloading {0}...", filename);
        progress = 0.01f;  // Set > 0 in order to show progress bar.

        // Hook the certificate-fixer callback to make TLS1.0 work (on some sites, anyway).
        ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;

        // Start the async download job.
        downloader = new WebClient();
        downloader.Encoding = Encoding.UTF8;
        downloader.DownloadProgressChanged += (sender, args) => { newPercentage = args.ProgressPercentage; };
        downloader.DownloadFileCompleted += (sender, args) => { ended = true; cancelled = args.Cancelled; error = args.Error; };
        downloader.DownloadFileAsync(new Uri(packageUrl), path);

        // Pause until download done/cancelled/fails, keeping progress bar up to date.
        while (!ended) {
            Repaint();
            yield return new WaitUntil(() => ended || newPercentage > oldPercentage);
            oldPercentage = newPercentage;
            progress = oldPercentage / 100.0f;
        }
        if (error != null) {
            Debug.LogError(error);
            cancelled = true;
        }

        // Reset async state so the GUI is operational again.
        downloader = null;
        coroutine = null;
        progress = 0;
        EditorUtility.ClearProgressBar();

        if (!cancelled)
            AssetDatabase.ImportPackage(path, true);  // OK, got the file, so let the user import it if they want.
        else
            Debug.Log("Download terminated.");
    }

    // Found the following workaround for TLS negotiation issues here:
    //   https://forum.unity.com/threads/how-to-properly-download-and-save-big-size-file.455384/#post-2975169
    // This occurs because of Unity's old (when using .NET < 4.5) TLS implementation which is no longer compatible
    // with some sites' download security policies.
    // This only seems to help in some cases (e.g. Firebase storage) but not others (e.g. Github).
    private static bool RemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate,
                                                            X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors != SslPolicyErrors.None &&
              chain.ChainStatus.Any(s => s.Status != X509ChainStatusFlags.RevocationStatusUnknown)) {
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
            return chain.Build((X509Certificate2)certificate);
        }
        return true;
    }
}
