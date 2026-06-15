using UnityEngine;

namespace VersionAssembly.Runtime
{
    [CreateAssetMenu(fileName = "VersionData", menuName = "Data/VersionData")]
    public class VersionData : ScriptableObject
    {
        public string version;
        public string gitHash;
        public string gitBranch;
    }
}
