using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Versionning.Editor
{
    public static class GitUtility
    {
        private static string ProjectRoot => Directory.GetParent(Application.dataPath).FullName;

        public static string RunCommand(string arguments)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo("git", arguments)
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = ProjectRoot
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd().Trim();
                string error = process.StandardError.ReadToEnd().Trim();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    if (!string.IsNullOrEmpty(error))
                        UnityEngine.Debug.LogError($"[GitUtility] git {arguments} -> {error}");
                    return string.Empty;
                }

                return output;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[GitUtility] Erreur sur 'git {arguments}' : {e.Message}");
                return string.Empty;
            }
        }

        public static string GetCurrentBranch() => RunCommand("branch --show-current");
        public static string GetCommitHash() => RunCommand("rev-parse HEAD");
        public static string GetShortCommitHash() => RunCommand("rev-parse --short HEAD");
        public static string GetCurrentTag() => RunCommand("describe --tags --abbrev=0");
        public static string GetStatus() => RunCommand("status --porcelain");
        public static bool IsClean() => string.IsNullOrEmpty(GetStatus());

        public static void CreateTag(string tag) => RunCommand($"tag {tag}");
        public static void PushTag(string tag) => RunCommand($"push origin {tag}");
    }
}