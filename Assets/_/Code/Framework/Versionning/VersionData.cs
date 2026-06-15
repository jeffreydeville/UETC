using UnityEngine;

namespace Versionning.Editor
{
    [CreateAssetMenu(fileName = "VersionData", menuName = "Data/VersionData")]
    public class VersionData : ScriptableObject
    {
        public string version;
        public string gitHash;
    }
}
