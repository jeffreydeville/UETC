using UnityEditor;
using UnityEngine;

namespace Versionning.Editor
{
    public class BuildManagerWindow : EditorWindow
    {
        private int m_Major;
        private int m_Minor;
        private int m_Patch;

        private string m_Branch = "";
        private string m_Tag = "";
        private string m_Commit = "";
        private bool m_IsClean = true;

        [MenuItem("Tools/Build Manager")]
        public static void ShowWindow()
        {
            GetWindow<BuildManagerWindow>("Build Manager");
        }

        private void OnEnable()
        {
            RefreshGitInfo();
        }

        private void OnGUI()
        {
            DrawGitInfoPanel();
            EditorGUILayout.Space(10);
            DrawVersionPanel();
            EditorGUILayout.Space(10);
            DrawActionsPanel();
        }

        private void DrawGitInfoPanel()
        {
            EditorGUILayout.LabelField("Informations Git", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Branche", m_Branch);
            EditorGUILayout.LabelField("Tag", m_Tag);
            EditorGUILayout.LabelField("Commit", m_Commit);
            EditorGUILayout.LabelField("Status", m_IsClean ? "Clean" : "Modified");

            if (!m_IsClean)
                EditorGUILayout.HelpBox("Le repo contient des changements non commités.", MessageType.Warning);

            if (GUILayout.Button("Rafraîchir"))
                RefreshGitInfo();
        }
        
        private void RefreshGitInfo()
        {
            m_Branch = GitUtility.GetCurrentBranch();
            m_Tag = GitUtility.GetCurrentTag();
            m_Commit = GitUtility.GetShortCommitHash();
            m_IsClean = GitUtility.IsClean();
        }

        private void DrawVersionPanel()
        {
            EditorGUILayout.LabelField("Nouvelle version", EditorStyles.boldLabel);

            m_Major = EditorGUILayout.IntField("Major", m_Major);
            m_Minor = EditorGUILayout.IntField("Minor", m_Minor);
            m_Patch = EditorGUILayout.IntField("Patch", m_Patch);

            EditorGUILayout.LabelField("Résultat", GetVersionString());
        }
        
        private string GetVersionString() => $"v{m_Major}.{m_Minor}.{m_Patch}";

        private void DrawActionsPanel()
        {
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

            using (new EditorGUI.DisabledScope(!m_IsClean))
            {
                if (GUILayout.Button("Create Tag"))
                    CreateTag();
            }

            if (!m_IsClean)
                EditorGUILayout.HelpBox("'Create Tag' désactivé tant que le repo n'est pas clean.", MessageType.Info);

            if (GUILayout.Button("Build Project"))
                BuildProcessor.BuildProject();
        } 

        private void CreateTag()
        {
            if (!GitUtility.IsClean())
            {
                Debug.LogError("[Build Manager] Repo pas clean, tag annulé.");
                return;
            }

            string tag = GetVersionString();
            GitUtility.CreateTag(tag);
            GitUtility.PushTag(tag);

            RefreshGitInfo();
        }
    }
}