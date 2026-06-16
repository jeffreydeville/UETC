using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VersionAssembly.Runtime;


namespace Versionning.Editor
{
    public class VersionAssetGenerator : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string VersionDataPath = "Assets/VersionData.asset";

        public void OnPreprocessBuild(BuildReport report)
        {
            UpdateVersionData();
        }

        public static void UpdateVersionData()
        {
            var data = AssetDatabase.LoadAssetAtPath<VersionData>(VersionDataPath);

            if (data == null)
            {
                Debug.LogError($"[Build Manager] VersionData introuvable à {VersionDataPath}");
                return;
            }

            string tag = GitUtility.GetCurrentTag();
            data.version = string.IsNullOrEmpty(tag) ? Application.version : tag.TrimStart('v', 'V');
            data.gitHash = GitUtility.GetShortCommitHash();
            data.gitBranch = GitUtility.GetCurrentBranch();
            data.buildDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
}