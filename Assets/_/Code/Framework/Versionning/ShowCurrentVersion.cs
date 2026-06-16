using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VersionAssembly.Runtime
{
    public class ShowCurrentVersion : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Label;
        [SerializeField] private VersionData m_VersionData;
        [SerializeField] private bool m_ShowVersion = true;
        [SerializeField] private bool m_ShowGitHash = true;
        [SerializeField] private bool m_ShowBranch = false;
        [SerializeField] private bool m_ShowBuildDate = false;

        private void Start()
        {
            var parts = new List<string>();
            if (m_ShowVersion) parts.Add($"v{m_VersionData.version}");
            if (m_ShowBranch) parts.Add(m_VersionData.gitBranch);
            if (m_ShowGitHash) parts.Add(m_VersionData.gitHash);
            if (m_ShowBuildDate) parts.Add(m_VersionData.buildDate);

            m_Label.text = string.Join(" · ", parts);
        }
    }
}