using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using OS = MoPubOSCommands;

public static class MoPubUpgradeMigration
{
    private static readonly Dictionary<string,string> LocationMapping = new Dictionary<string, string>
    {
        { "MoPub/Editor", "MoPub/Scripts/Editor" },
        { "MoPub/Internal", "MoPub/Scripts/Internal" },
        { "MoPub/ThirdParty", "MoPub/Scripts/ThirdParty" },
        { "MoPub/MoPub.cs", "MoPub/Scripts/MoPub.cs" },
        { "MoPub/MoPubManager.cs", "MoPub/Scripts/MoPubManager.cs" },
        { "MoPub/mopub-dark-logo.png", "MoPub/Sample/mopub-dark-logo.png" },
        { "Scenes/MoPubDemoScene.unity", "MoPub/Sample/MoPubDemoScene.unity" },
        { "Scripts/GravityController.cs", "MoPub/Sample/GravityController.cs" },
        { "Scripts/MoPubDemoGUI.cs", "MoPub/Sample/MoPubDemoGUI.cs" },
        { "Scripts/MoPubEventListener.cs", "MoPub/Sample/MoPubEventListener.cs" },
        { "Plugins/Android/mopub", "MoPub/Plugins/Android/MoPub.plugin" },
        { "Plugins/Android/README.txt", "MoPub/Plugins/Android/MoPub.plugin/README.txt" },
        { "Plugins/iOS/MoPubBinding.m", "MoPub/Plugins/iOS/MoPubBinding.m" },
        { "Plugins/iOS/MoPubManager.h", "MoPub/Plugins/iOS/MoPubManager.h" },
        { "Plugins/iOS/MoPubManager.mm", "MoPub/Plugins/iOS/MoPubManager.mm" },
        { "Plugins/iOS/MoPubSDKFramework.framework", "MoPub/Plugins/iOS/MoPubSDKFramework.framework" },
        // Mediation directories will be handled on later releases
        //{ "Plugins/Android/mopub-support", "MoPub/Plugins/Android/MoPub.plugin/" }
    };
    private static readonly Dictionary<string,string> ManualMapping = new Dictionary<string, string>
    {
        { "Plugins/Android/res/drawable/ic_launcher_foreground.png", "MoPub/Plugins/Android/MoPub.plugin/res/drawable/" },
        { "Plugins/Android/res/drawable-anydpi-v26/app_icon.xml", "MoPub/Plugins/Android/MoPub.plugin/res/drawable-anydpi-v26/" },
        { "Plugins/Android/res/values/ic_launcher_background.xml", "MoPub/Plugins/Android/MoPub.plugin/res/values/" },
    };
    private const string RedundantDir = "Assets/MoPub/Extras/";
    private const string MigrationBegin = "==========> Beginning MoPub Upgrade Migration";
    private const string MigrationEnd = "==========> Finished MoPub Upgrade Migration";
    private const string ManualMigrationNote =
        "The following files cannot be automatically migrated since they may contain non-MoPub code.\nPlease review this list and manually migrate if needed:\n";
    private const string ManualFollowUpWarning =
        "Some files were not able to be migrated! Please scroll up and handle manually as needed.";

    public static bool LegacyMoPubPresent()
    {
        return LocationMapping.Keys.Select(p => DoesExist("Assets/" + p)).Any(p => p);
    }

    public static void DoMigration()
    {
        Debug.Log(MigrationBegin);

        if (Directory.Exists(RedundantDir)) OS.RmDir(RedundantDir);

        var allSucceeded = true;

        foreach (var entry in LocationMapping) {
            var source = Path.Combine("Assets", entry.Key);
            var dest = Path.Combine("Assets", entry.Value);
            if (!DoesExist(source)) continue;
            allSucceeded &= OS.Mv(source, dest);
        }

        bool showNote = false;
        var migrationNote = new StringBuilder(ManualMigrationNote);
        foreach (var entry in ManualMapping)
            if (DoesExist(Path.Combine("Assets", entry.Key))) {
                showNote = true;
                migrationNote.Append(string.Format("'{0}' to '{1}'\n", entry.Key, entry.Value));
            }
        if (showNote)
            Debug.LogWarning(migrationNote);

        if (!allSucceeded)
            Debug.LogWarning(ManualFollowUpWarning);
        AssetDatabase.Refresh();
        Debug.Log(MigrationEnd);
    }

    private static bool DoesExist(string path)
    {
        return File.Exists(path)
               || Directory.Exists(path)
               || path.EndsWith("/*") && Directory.Exists(Path.GetDirectoryName(path));
    }
}
