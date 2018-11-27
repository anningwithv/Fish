using UnityEditor;
using UnityEngine;

public class MoPubAboutDialog : ScriptableWizard
{
    public static void ShowDialog()
    {
        DisplayWizard<MoPubAboutDialog>("About MoPub SDK", "OK");
    }

    protected override bool DrawWizardGUI()
    {
        EditorGUILayout.LabelField("MoPub SDK version " + MoPub.moPubSDKVersion);

        EditorGUILayout.Space();
        if (GUILayout.Button("Release Notes"))
            Application.OpenURL("https://github.com/mopub/mopub-unity-sdk/tree/v" + MoPub.moPubSDKVersion);

        return false;
    }

    private void OnWizardCreate() { }
}
