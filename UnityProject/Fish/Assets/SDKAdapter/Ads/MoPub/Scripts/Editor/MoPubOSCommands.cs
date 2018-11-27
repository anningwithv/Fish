using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

public class MoPubOSCommands {

    // Helper function to run an external command and maybe capture the output of stdout.  The command name in exe will
    // be path-searched if necessary.  The command is run with the current directory set to 'dir' and the environment
    // augmented with any values passed in vars[] (each must be of the form "VAR=VALUE").
    public static bool RunExe(StringBuilder stdout, string exe, string args, string dir, string[] vars)
    {
#if UNITY_EDITOR_WIN
        // Convenience since lots of exes on Windows end with ".exe" when the corresponding exe on Mac ends with nothing.
        if (string.IsNullOrEmpty(Path.GetExtension(exe)))
            exe += ".exe";
#endif // UNITY_EDITOR_WIN

        var proc = new Process { StartInfo = new ProcessStartInfo {
            FileName = exe,
            Arguments = args ?? string.Empty,
            WorkingDirectory = dir ?? string.Empty,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = stdout != null,
            RedirectStandardError = true,
        }};
        foreach (var vv in vars.Select(v => v.Split('='))) {
            proc.StartInfo.EnvironmentVariables.Add(vv[0], vv[1]);
        }

        if (stdout != null)
            proc.OutputDataReceived += (sender, arg) => stdout.AppendLine(arg.Data);
        var stderr = new StringBuilder();
        proc.ErrorDataReceived += (sender, arg) => stderr.AppendLine(arg.Data);

        try {
            proc.Start();
            if (stdout != null)
                proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();
            if (proc.ExitCode != 0) {
                Debug.LogErrorFormat("Error exit while running '{0} {1}': {2}\n{3}", exe, args, proc.ExitCode, stderr);
                return false;
            }
        } catch (Exception e) {
            Debug.LogErrorFormat("Exception while running '{0} {1}': {2}", exe, args, e);
            return false;
        }
        return true;
    }


    // Helper wrapper function for RunExe for when you are capturing output of stdout.  Returns the output as a string.
    // If the command succeeded (exit code 0), this will never be null.  Otherwise (exit code != 0) it will be null.
    public static string RunForOutput(string exe, string args = null, string dir = null, params string[] vars)
    {
        var stdout = new StringBuilder();
        return RunExe(stdout, exe, args, dir, vars) ? stdout.ToString() : null;
    }


    // Helper wrapper function for RunExe for when you don't want to capture stdout, only the success/failure of the
    // command.  This returns true if the command's exit code was 0, otherwise false.
    public static bool Run(string exe, string args = null, string dir = null, params string[] vars)
    {
        return RunExe(null, exe, args, dir, vars);
    }


    // Helper function to create dest directory, used in other functions below.  Normal usage is to call
    // MkDir(Path.GetDirectory(destFilePath)), to ensure that the file-to-be-created has a directory in which to be
    // created. Hence no action is taken for an empty path, since that's what you get when the file is going into the
    // current working directory.
    public static void Mkdir(string path)
    {
        if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            Directory.CreateDirectory(path);
    }


    // Simple analog of the Linux/BSD "cp" command.
    internal static bool Cp(string source, string dest)
    {
        try {
            Mkdir(Path.GetDirectoryName(dest));
            File.Copy(source, dest, overwrite:true);
            return true;
        } catch (Exception e) {
            Debug.LogErrorFormat("Exception while copying file '{0}' to '{1}': {2}", source, dest, e);
            return false;
        }
    }


    // Simple analog of the Linux/BSD "mv" command.  If source is...
    // > a file (ending in anything BUT / or *), it will be moved to dest.
    // > a directory (ending in /), it will be recursively moved to dest.
    // > a directory with wildcard (ending in /*), the directory contents will be recursively moved to dest, then the
    // remaining empty directory is removed.
    public static bool Mv(string source, string dest)
    {
        if (source.EndsWith("/*")) {
            source = Path.GetDirectoryName(source);
            if (!dest.EndsWith("/")) // Make sure dest is treated as a directory.
                dest += "/";
            Debug.LogFormat("Moving directory contents from '{0}' to '{1}'", source, dest);
            var result = Find(source, recursive: false)
                        .Select(p => Path.Combine(source, p))
                        .All(p => Mv(p, dest))
                     && RmDir(source);
            if (result)
                Rm(source + ".meta");
            return result;
        }

        if (!File.Exists(source) && !Directory.Exists(source)) {
            Debug.LogWarningFormat("Source doesn't exist: {0}", source);
            return false;
        }
        if (string.IsNullOrEmpty(Path.GetFileName(dest)) || File.Exists(source) && Directory.Exists(dest))
            dest = Path.Combine(dest, Path.GetFileName(source));

        if (File.Exists(dest) || Directory.Exists(dest)) {
            Debug.LogWarningFormat("Ignoring, destination already exists: {0}", dest);
            return false;
        }

        Debug.LogFormat("Moving '{0}' to '{1}'", source, dest);
        try {
            Mkdir(Path.GetDirectoryName(dest));  // Parent dirs in dest.
            Directory.Move(source, dest);  // This works if source is file or directory.
            source += ".meta";
            dest += ".meta";
            if (File.Exists(source) && !File.Exists(dest))
                Directory.Move(source, dest);
            return true;
        } catch (Exception e) {
            Debug.LogErrorFormat("Exception while moving '{0}' to '{1}': {2}", source, dest, e);
            return false;
        }
    }


    // Simple analog of the Linux/BSD "rm" command.
    internal static bool Rm(string path)
    {
        try {
            Debug.Log("Removing file: " + path);
            File.Delete(path);
            path += ".meta";
            if (File.Exists(path)) File.Delete(path);
            return true;
        } catch (Exception e) {
            Debug.LogErrorFormat("Exception while deleting file '{0}': {1}", path, e);
            return false;
        }
    }


    // Simple analog of the Linux/BSD "rmdir" command.
    public static bool RmDir(string path)
    {
        Debug.Log("Removing directory: " + path);
        if (!Directory.Exists(path))
        {
            Debug.LogWarningFormat("Ignoring, directory not found: {0}", path);
            return false;
        }
        try {
            Directory.Delete(path, true);
            if (string.IsNullOrEmpty(Path.GetFileName(path)))
                path = Path.GetDirectoryName(path);
            path += ".meta";
            if (File.Exists(path))
                File.Delete(path);
        } catch (Exception e) {
            Debug.LogErrorFormat("Exception while deleting '{0}': {1}", path, e);
            return false;
        }
        return true;
    }


    // Simple analog of the Linux/BSD "sed" command, using the -i flag to edit a file in place.
    public static bool Sed(string file, string regex, string repl)
    {
        try {
            var text = File.ReadAllText(file);
            text = Regex.Replace(text, regex, repl, RegexOptions.Multiline);
            File.WriteAllText(file, text);
            return true;
        } catch (Exception e) {
            Debug.LogErrorFormat("Exception while rewriting file {0}: {1}", file, e);
            return false;
        }
    }


    // MSDN docs say that Directory.GetFileSystemEntries() returns the "names" of the directory's contents,
    // but we're getting back full paths.  This wrapper works around that because we need the simple names.
    // Both overloads of Directory.GetFileSystemEntries() -- with or without a search pattern -- are supported.
    public static IEnumerable<string> GetFileSystemEntries(string dir, string pattern = null)
    {
        try {
            return string.IsNullOrEmpty(pattern)
                ? from name in Directory.GetFileSystemEntries(dir) select Path.GetFileName(name)
                : from name in Directory.GetFileSystemEntries(dir, pattern) select Path.GetFileName(name);
        } catch (Exception e) {
            Debug.LogErrorFormat("Exception while finding files in dir {0}: {1}", dir, e);
            return Enumerable.Empty<string>();
        }
    }


    // Simple analog of the Linux/BSD "find" command.  It is equivalent to just "find dir" with flags "-type f"
    // and/or "-type d" added.  However the returned paths are relative to dir, unlike the find command.
    public static IEnumerable<string> Find(string dir, bool files = true, bool dirs = true, bool recursive = true)
    {
        if (!Directory.Exists(dir))
            yield break;
        foreach (var name in GetFileSystemEntries(dir)) {
            var path = Path.Combine(dir, name);
            if (File.Exists(path)) {
                if (files)
                    yield return name;
            } else if (Directory.Exists(path)) {
                if (dirs)
                    yield return name;
                if (!recursive) continue;
                foreach (var subPath in Find(path, files, dirs))
                    yield return Path.Combine(name, subPath);
            }
        }
    }


    // Simple analog of "rsync" command.  Note that each entry in names[] can contain a wildcard, like '*.png'.
    // If it refers to a directory (e.g., '*.framework' on a Mac) the whole directory is copied into the destination
    // recursively, and any files present in the destination that don't correspond to files in the source are deleted.
    // (.meta files are excepted for Unity friendliness).  In effect we are doing this:
    //    rsync -r --delete --exclude='*.meta' srcDir/DIR destDir/
    public static bool Rsync(string srcDir, string destDir, params string[] names)
    {
        // Expand wildcards.
        names = names.SelectMany(name => GetFileSystemEntries(srcDir, name)).ToArray();
        if (names.Length == 0) {
            Debug.LogErrorFormat("No matching files found in dir {0}", srcDir);
            return false;
        }

        foreach (var name in names) {
            var srcPath = Path.Combine(srcDir, name);
            var destPath = Path.Combine(destDir, name);
            if (File.Exists(srcPath)) {
                // This means: if there's a directory at the destination path where the src file wants to be, first try
                // to delete it (unlikely but has been known to occur).  Then copy the file over.  Stop if any of these
                // steps fail.
                if (Directory.Exists(destPath) && !RmDir(destPath)  // Unlikely, but has been known to occur...
                    || !Cp(srcPath, destPath))
                    return false;
            } else if (Directory.Exists(srcPath)) {
                // Look for stale paths in the destination directory (left over from a prior build, but some files have
                // since been renamed or removed).  Unity .meta files are handled along with their associated asset
                // file, rather than separately.  (Otherwise they'd all get deleted every time we build, and Unity would
                // generate all new ones with nothing but a different timestamp, which clogs up git diffs pointlessly.)
                var staleDestRms = from subPath in Find(destPath)
                                   where !subPath.EndsWith(".meta")
                                   let src = Path.Combine(srcPath, subPath)
                                   let dest = Path.Combine(destPath, subPath)
                                   let meta = dest + ".meta"
                                   select File.Exists(dest)
                                          ? File.Exists(src) || Rm(dest) && Rm(meta)
                                          : Directory.Exists(src) || RmDir(dest) && Rm(meta);
                // Copy all files in the src directory tree to the destination.
                var copySrcFiles = from subPath in Find(srcPath, dirs:false)
                                   let src = Path.Combine(srcPath, subPath)
                                   let dest = Path.Combine(destPath, subPath)
                                   select Cp(src, dest);
                // Fail if any of the above operations fails.
                if (staleDestRms.Contains(false) || copySrcFiles.Contains(false))
                    return false;
            }
        }
        return true;
    }
}
