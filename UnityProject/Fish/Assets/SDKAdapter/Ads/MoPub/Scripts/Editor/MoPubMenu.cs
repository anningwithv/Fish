using UnityEditor;
using UnityEngine;

public class MoPubMenu
{
    public const string MoPubBuildInternalPref = "MoPubBuildInternal";
    public const string MoPubBuildDebugPref = "MoPubBuildDebug";
    public const string MoPubBuildOnDemandPref = "MoPubBuildOnDemand";
    private const string MopubBuildOption = "MoPub/Build/Option: ";

#if !mopub_disable_menu

    [MenuItem("MoPub/About MoPub SDK", false, 0)]
    public static void About()
    {
        MoPubAboutDialog.ShowDialog();
    }

    [MenuItem("MoPub/Documentation...", false, 1)]
    public static void Documentation()
    {
        Application.OpenURL("https://developers.mopub.com/docs/unity/");
    }

    [MenuItem("MoPub/Report Issue...", false, 2)]
    public static void ReportIssue()
    {
        Application.OpenURL("https://github.com/mopub/mopub-unity-sdk/issues");
    }

    [MenuItem("MoPub/Manage SDKs...", false, 3)]
    public static void SdkManager()
    {
        MoPubSDKManager.ShowSDKManager();
    }


#if mopub_build_menu_beta
    [MenuItem("MoPub/Build/Build Current Platform", false, 10)]
    public static bool BuildMoPubSdkCurrent()
    {
        return MoPubSDKBuild.BuildMoPubSdk(EditorUserBuildSettings.activeBuildTarget);
    }


    [MenuItem("MoPub/Build/Build Current Platform", true)]
    public static bool ValidateBuildMoPubSdkCurrent()
    {
        return EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android
#if UNITY_EDITOR_OSX
            || EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS
#endif // UNITY_EDITOR_OSX
            ;
    }


    [MenuItem("MoPub/Build/Build All Platforms", false, 10)]
    public static bool BuildMoPubSdkAll()
    {
        return MoPubSDKBuild.BuildMoPubSdk(BuildTarget.Android) && MoPubSDKBuild.BuildMoPubSdk(BuildTarget.iOS);
    }


    [MenuItem("MoPub/Build/Build All Platforms", true)]
    public static bool ValidateBuildMoPubSdkAll()
    {
        return
#if UNITY_EDITOR_OSX
            true
#else
            false
#endif // UNITY_EDITOR_OSX
            ;
    }


    [MenuItem("MoPub/Build/Export MoPub Package", false, 12)]
    public static bool ExportPackage()
    {
        return MoPubSDKBuild.ExportMoPubPackage();
    }


    #region Options


#if mopub_developer

    //////  Use Public SDK

    private const string MoPubBuildInternalOption = MopubBuildOption + "Use Public SDK/";
    private const string MoPubBuildInternalOptionAlways = MoPubBuildInternalOption + "Always";
    private const string MoPubBuildInternalOptionNever = MoPubBuildInternalOption + "Never";
    private const string MoPubBuildInternalOptionReleaseBuild = MoPubBuildInternalOption + "If Release Build";


    [MenuItem(MoPubBuildInternalOptionAlways, false, 20)]
    public static void ForceInternalBuild()
    {
        EditorPrefs.SetBool(MoPubBuildInternalPref, true);
    }


    [MenuItem(MoPubBuildInternalOptionAlways, true)]
    public static bool ValidateForceInternalBuild()
    {
        var isChecked = EditorPrefs.GetBool(MoPubBuildInternalPref, false);
        Menu.SetChecked(MoPubBuildInternalOptionAlways, isChecked);
        return true;
    }


    [MenuItem(MoPubBuildInternalOptionNever, false, 21)]
    public static void ForcePublicBuild()
    {
        EditorPrefs.SetBool(MoPubBuildInternalPref, false);
    }


    [MenuItem(MoPubBuildInternalOptionNever, true)]
    public static bool ValidateForcePublicBuild()
    {
        var isChecked = !EditorPrefs.GetBool(MoPubBuildInternalPref, true);
        Menu.SetChecked(MoPubBuildInternalOptionNever, isChecked);
        return true;
    }


    [MenuItem(MoPubBuildInternalOptionReleaseBuild, false, 22)]
    public static void InternalBuildInDevelopmentMode()
    {
        EditorPrefs.DeleteKey(MoPubBuildInternalPref);
    }


    [MenuItem(MoPubBuildInternalOptionReleaseBuild, true)]
    public static bool ValidateInternalBuildInDevelopmentMode()
    {
        var isChecked = !EditorPrefs.HasKey(MoPubBuildInternalPref);
        Menu.SetChecked(MoPubBuildInternalOptionReleaseBuild, isChecked);
        return true;
    }
#endif // mopub_developer

    //////  Debug

    private const string MoPubBuildDebugOption = MopubBuildOption + "Debug Build/";
    private const string MoPubBuildDebugOptionAlways = MoPubBuildDebugOption + "Always";
    private const string MoPubBuildDebugOptionNever = MoPubBuildDebugOption + "Never";
    private const string MoPubBuildDebugOptionDevelopmentBuild = MoPubBuildDebugOption + "If Development Build";


    [MenuItem(MoPubBuildDebugOptionAlways, false, 23)]
    public static void ForceDebugBuild()
    {
        EditorPrefs.SetBool(MoPubBuildDebugPref, true);
    }


    [MenuItem(MoPubBuildDebugOptionAlways, true)]
    public static bool ValidateForceDebugBuild()
    {
        var isChecked = EditorPrefs.GetBool(MoPubBuildDebugPref, false);
        Menu.SetChecked(MoPubBuildDebugOptionAlways, isChecked);
        return true;
    }


    [MenuItem(MoPubBuildDebugOptionNever, false, 24)]
    public static void ForceReleaseBuild()
    {
        EditorPrefs.SetBool(MoPubBuildDebugPref, false);
    }


    [MenuItem(MoPubBuildDebugOptionNever, true)]
    public static bool ValidateForceReleaseBuild()
    {
        var isChecked = !EditorPrefs.GetBool(MoPubBuildDebugPref, true);
        Menu.SetChecked(MoPubBuildDebugOptionNever, isChecked);
        return true;
    }


    [MenuItem(MoPubBuildDebugOptionDevelopmentBuild, false, 25)]
    public static void DebugBuildInDevelopmentMode()
    {
        EditorPrefs.DeleteKey(MoPubBuildDebugPref);
    }


    [MenuItem(MoPubBuildDebugOptionDevelopmentBuild, true)]
    public static bool ValidateDebugBuildInDevelopmentMode()
    {
        var isChecked = !EditorPrefs.HasKey(MoPubBuildDebugPref);
        Menu.SetChecked(MoPubBuildDebugOptionDevelopmentBuild, isChecked);
        return true;
    }


    //////  On Demand

    private const string MoPubBuildOnDemandOption = MopubBuildOption + "Compile on App Build";


    [MenuItem(MoPubBuildOnDemandOption, false, 26)]
    public static void ToggleOnDemandBuild()
    {
        var onDemand = EditorPrefs.GetBool(MoPubBuildOnDemandPref, false);
        EditorPrefs.SetBool(MoPubBuildOnDemandPref, !onDemand);
    }


    [MenuItem(MoPubBuildOnDemandOption, true)]
    public static bool ValidateToggleOnDemandBuild()
    {
        var isChecked = EditorPrefs.GetBool(MoPubBuildOnDemandPref, false);
        Menu.SetChecked(MoPubBuildOnDemandOption, isChecked);
        return true;
    }

    #endregion

#endif // mopub_build_menu_beta

#endif // mopub_disable_menu


}
