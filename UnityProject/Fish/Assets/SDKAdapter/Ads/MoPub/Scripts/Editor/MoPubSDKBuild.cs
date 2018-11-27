using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_5_6_OR_NEWER
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif // UNITY_2018_1_OR_NEWER
#endif // UNITY_5_6_OR_NEWER
using OS = MoPubOSCommands;

public static class MoPubSDKBuild
{
    #region Platform Specific Values


    // Returns the full (absolute) path of a subdirectory of the MoPub unity repo.  Since the unity-sample-app subdir is
    // where Unity is running, we have to include a ".." to adjust for that.
    private static string GetFullPath(string subdir)
    {
        return Path.GetFullPath(Path.Combine("..", subdir));
    }


    // The path to the platform MoPub SDK (Android or iOS, internal or public subrepo).
    private static string GetSdkSubdir(BuildTarget buildTarget, bool internalSdkBuild)
    {
        var sdkDir = "mopub";
        switch (buildTarget) {
            case BuildTarget.Android:
                sdkDir += "-android";
                break;
            case BuildTarget.iOS:
                sdkDir += "-ios";
                break;
            default:
                Debug.LogError("Invalid build target: " + buildTarget);
                return null;
        }
        if (!internalSdkBuild)
            sdkDir += "-sdk";
        return GetFullPath(sdkDir);
    }


    // The file within the platform MoPub SDK that contains the SDK's version string constant.
    private static readonly Dictionary<BuildTarget, string> FileWithVersionNumber = new Dictionary<BuildTarget, string>
    {
        {BuildTarget.Android, "mopub-sdk/mopub-sdk-base/src/main/java/com/mopub/common/MoPub.java"},
        {BuildTarget.iOS, "MoPubSDK/MPConstants.h"}
    };


    // The suffix to add to the platform MoPub SDK's version string.  For public builds this is simply "unity".  For
    // internal development builds, it is the git commit (short) SHA that we are building against.
    private static string GetSdkVersionSuffix(BuildTarget buildTarget, bool internalSdkBuild)
    {
        if (!internalSdkBuild)
            return "unity";
        // cd to appropriate git submodule directory and get the (shortened) SHA of the latest commit
        var dir = GetSdkSubdir(buildTarget, internalSdkBuild);
        var githash = OS.RunForOutput("git", "rev-parse --short HEAD", dir) ?? "unknown";
        return githash.Trim();
    }


    // A regex that matches the part of the version string occupied by our Unity suffix.  Uses lookbehind and lookahead
    // anchors to locate the spot.  The resulting matched substring will be zero-length when no suffix is already in place
    // (the usual case on a fresh checkout).  But that's fine since the Regex.Replace() function knows where in the string
    // the match happened regardless.
    private static readonly Dictionary<BuildTarget, string> RegexForVersionLine = new Dictionary<BuildTarget, string>
    {
        { BuildTarget.Android, @"(?<=public static final String SDK_VERSION[^""]*""[^+""]*)(\+[^""]*)?(?="")" },
        { BuildTarget.iOS,     @"(?<=#define MP_SDK_VERSION[^""]*""[^+""]*)(\+[^""]*)?(?="")" }
    };


    #endregion

    #region Build Steps


    // Each platform SDK has a file that contains a version string constant which identifies the current build.
    // We append a suffix to this version string to identify that the platform SDK is being used from Unity rather
    // than standalone.
    private static bool AppendUnityVersion(BuildTarget buildTarget, bool internalSdkBuild)
    {
        Debug.Log("Appending Unity marker to MoPub platform SDK version string");
        var file = Path.Combine(GetSdkSubdir(buildTarget, internalSdkBuild), FileWithVersionNumber[buildTarget]);
        var regex = RegexForVersionLine[buildTarget];
        var repl = "+" + GetSdkVersionSuffix(buildTarget, internalSdkBuild);
        return OS.Sed(file, regex, repl);
    }


    // Build the platform MoPub SDK using its native build system.
    private static bool BuildPlatformSdk(BuildTarget buildTarget, bool internalSdkBuild, bool debug)
    {
        Debug.LogFormat("Building MoPub platform SDK for {0} (internal: {1}, debug: {2})", buildTarget, internalSdkBuild, debug);
        switch (buildTarget) {
            case BuildTarget.Android:
#if UNITY_EDITOR_OSX
                const string cmd = "bash";
                const string gradlew = "gradlew";
#elif UNITY_EDITOR_WIN
                const string cmd = "cmd";
                const string gradlew = "/c gradlew.bat";
#endif // UNITY_EDITOR_OSX
                var sdkDir = GetSdkSubdir(buildTarget, internalSdkBuild);
                return OS.Run(cmd, string.Format("{0} clean assemble{1}", gradlew, debug ? "Debug" : "Release"),
                           GetFullPath("mopub-android-sdk-unity"),
                           "SDK_DIR=" + Path.GetFileName(sdkDir),
                           "ANDROID_HOME=" + EditorPrefs.GetString("AndroidSdkRoot"));
#if UNITY_EDITOR_OSX
            case BuildTarget.iOS:
                var project = "mopub-ios-sdk-unity.xcodeproj";
                if (internalSdkBuild)
                    project = "internal-" + project;
                var jsfile = GetFullPath("mopub-ios-sdk-unity/bin/MoPubSDKFramework.framework/MRAID.bundle/mraid.js");
                return OS.Run("xcrun",
                           "xcodebuild"
                           + " -project " + project
                           + " -scheme \"MoPub for Unity\""
                           + " -configuration \"" + (debug ? "Debug" : "Release") + "\""
                           + " OTHER_CFLAGS=\"-fembed-bitcode -w\""
                           + " BITCODE_GENERATION_MODE=bitcode"
                           + " clean build",
                           GetFullPath("mopub-ios-sdk-unity"))
                    // Have to rename the .js file inside the framework so that Unity doesn't try to compile it...
                    // This rename is reverted in the MoPubPostBuildiOS script during an app build.
                    && OS.Mv(jsfile, jsfile + ".prevent_unity_compilation");
#endif // UNITY_EDITOR_OSX
            default:
                Debug.LogError("Invalid build target: " + buildTarget);
                return false;
        }
    }


    // Undo the edit to the platform MoPub SDK's source file that contains the version string.
    private static bool RestoreVersionFile(BuildTarget buildTarget, bool internalSdkBuild)
    {
        Debug.Log("Restoring platform SDK's version file");
        var dir = GetSdkSubdir(buildTarget, internalSdkBuild);
        var file = FileWithVersionNumber[buildTarget];
        return OS.Run("git", "checkout -- " + file, dir);
    }


    private static bool CopyBuildArtifacts(BuildTarget buildTarget, bool internalSdkBuild, bool debug)
    {
        Debug.Log("Copying MoPub libraries to Unity project");
        var sdkDir = GetSdkSubdir(buildTarget, internalSdkBuild);
        switch (buildTarget) {
            case BuildTarget.Android: {
                const string destDir = "Assets/MoPub/Plugins/Android/MoPub.plugin";
                // Our wrapper jar.
                var jarDir = debug ? "debug" : "release";
                if (!OS.Cp(GetFullPath("mopub-android-sdk-unity/build/intermediates/bundles/" + jarDir + "/classes.jar"),
                    Path.Combine(destDir, "libs/mopub-unity-wrappers.jar")))
                    return false;
                // Platform SDK jars.
                var libCps = from lib in new[] { "base", "banner", "interstitial", "rewardedvideo", "native-static" }
                             let src = string.Format("{0}/mopub-sdk/mopub-sdk-{1}/build/intermediates/bundles/{2}/classes.jar",
                                                     sdkDir, lib, jarDir)
                             let dst = string.Format("{0}/libs/mopub-sdk-{1}.jar", destDir, lib)
                             select OS.Cp(src, dst);
                if (libCps.Contains(false))
                    return false;
                return true;
            }
#if UNITY_EDITOR_OSX
            case BuildTarget.iOS: {
                const string destDir = "Assets/MoPub/Plugins/iOS";
                var projDir = GetFullPath("mopub-ios-sdk-unity/bin");
                var htmlDir = Path.Combine(sdkDir, "MoPubSDK/Resources");
                return OS.Rsync(projDir, destDir, "*.h", "*.m", "*.mm", "*.framework")
                    && OS.Rsync(htmlDir, Path.Combine(destDir, "MoPubSDKFramework.framework"), "*.html", "*.png");
            }
#endif // UNITY_EDITOR_OSX
            default:
                Debug.LogError("Invalid build target: " + buildTarget);
                return false;
        }
    }


    #endregion

    #region Build Hooks

    public static bool BuildMoPubSdk(BuildTarget buildTarget)
    {
#if mopub_developer
        var internalSdkBuild =
            EditorPrefs.GetBool(MoPubMenu.MoPubBuildInternalPref, EditorUserBuildSettings.development);
#else
        const bool internalSdkBuild = false;
#endif // mopub_developer
        var debug = EditorPrefs.GetBool(MoPubMenu.MoPubBuildDebugPref, EditorUserBuildSettings.development);

        if (!AppendUnityVersion(buildTarget, internalSdkBuild))
            return false;

        // Hang on to this since we want to undo the version edit even if the build fails.
        var built = BuildPlatformSdk(buildTarget, internalSdkBuild, debug);
        return RestoreVersionFile(buildTarget, internalSdkBuild)
               && built
               && CopyBuildArtifacts(buildTarget, internalSdkBuild, debug);
    }


    public static bool ExportMoPubPackage()
    {
        var build = EditorPrefs.GetBool(MoPubMenu.MoPubBuildOnDemandPref, false);
        if (build) {
            if (!BuildMoPubSdk(BuildTarget.Android))
                return false;
#if UNITY_EDITOR_OSX
            if (!BuildMoPubSdk(BuildTarget.iOS))
                return false;
#endif // UNITY_EDITOR_OSX
        }

        Debug.Log("Exporting package");
        AssetDatabase.ExportPackage(new[] {"Assets/MoPub"},
            GetFullPath("mopub-unity-plugin/MoPubUnity.unitypackage"),
            ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        return true;
    }


#if UNITY_5_6_OR_NEWER
    internal class BuildProcessor :
#if UNITY_2018_1_OR_NEWER
        IPreprocessBuildWithReport
#else
        IPreprocessBuild
#endif // UNITY_2018_1_OR_NEWER
    {
        // Controls order of execution among multiple IOrderedCallback implementations
        // https://docs.unity3d.com/ScriptReference/Build.IOrderedCallback.html
        public int callbackOrder
        {
            get { return 0; }
        }

#if UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(BuildReport report)
        {
            var target = report.summary.platform;
#else
        public void OnPreprocessBuild(BuildTarget target, string path)
        {
#endif // UNITY_2018_1_OR_NEWER
            var build = EditorPrefs.GetBool(MoPubMenu.MoPubBuildOnDemandPref, false);
#if UNITY_EDITOR_WIN

// Can't build iOS SDK on Windows
            if (target == BuildTarget.iOS)
                build = false;
#endif // UNITY_EDITOR_WIN
            if (build)
                BuildMoPubSdk(target);
        }
    }
#endif // UNITY_5_6_OR_NEWER

    #endregion
}
