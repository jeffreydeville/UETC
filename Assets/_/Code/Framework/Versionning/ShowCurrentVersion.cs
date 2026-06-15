using TMPro;
using UnityEngine;
using VersionAssembly.Runtime;

namespace Versionning
{
    public class ShowCurrentVersion : MonoBehaviour
    {
        #region Publics
        #endregion
        
        #region Unity API
        [SerializeField] private TextMeshProUGUI m_Label;
        [SerializeField] private VersionData m_VersionData;
        [SerializeField] private bool m_ShowVersion = true;
        [SerializeField] private bool m_ShowGitHash = true;
        
        private void Start()
        {
            m_Label.text = (m_ShowVersion ? $"v{m_VersionData.version}" : "")
                           + (m_ShowVersion && m_ShowGitHash ? " · " : "")
                           + (m_ShowGitHash ? m_VersionData.gitHash : "");
        }
        #endregion
        
        
    }
}