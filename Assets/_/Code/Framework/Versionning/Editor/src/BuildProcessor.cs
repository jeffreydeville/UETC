using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Versionning.Editor
{
    public static class BuildProcessor
    {
        public static void BuildProject()
        {
            string tag = GitUtility.GetCurrentTag();
            if (string.IsNullOrEmpty(tag))
            {
                tag = "v" + Application.version;
                Debug.LogWarning($"[Build Manager] Aucun tag git trouvé, fallback sur {tag}");
            }

            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError("[Build Manager] Aucune scène activée dans Build Settings.");
                return;
            }

            string outputFolder = Path.Combine("Builds", tag);
            Directory.CreateDirectory(outputFolder);
            string outputPath = Path.Combine(outputFolder, $"{PlayerSettings.productName}.exe");

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = outputPath,
                target = EditorUserBuildSettings.activeBuildTarget,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);

            if (report.summary.result == BuildResult.Succeeded)
                Debug.Log($"[Build Manager] Build réussi : {outputPath}");
            else
                Debug.LogError($"[Build Manager] Build échoué : {report.summary.result}");
        }
    }
}