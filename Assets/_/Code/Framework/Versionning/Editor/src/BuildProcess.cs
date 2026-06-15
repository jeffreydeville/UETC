using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Diagnostics;
using VersionAssembly.Runtime;

public class BuildProcess : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    

    public void OnPreprocessBuild(BuildReport report)
    {
        var data = AssetDatabase.LoadAssetAtPath<VersionData>("Assets/VersionData.asset");
        if (data == null)
        {
            UnityEngine.Debug.LogError("VersionData introuvable à Assets/Data/VersionData.asset");
            return;
        }

        data.version = Application.version;
        data.gitHash = GetGitHash();
        data.gitBranch = GetGitBranch();
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
    }

    private static string GetGitHash()
    {
        var p = new Process
        {
            StartInfo = new ProcessStartInfo("git", "rev-parse --short HEAD")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        p.Start();
        string hash = p.StandardOutput.ReadToEnd().Trim();
        p.WaitForExit();
        return hash;
    }
    private static string GetGitBranch()
    {
        var p = new Process
        {
            StartInfo = new ProcessStartInfo("git", "rev-parse --abbrev-ref HEAD")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        p.Start();
        string branch = p.StandardOutput.ReadToEnd().Trim();
        p.WaitForExit();
        return branch;
    }
}