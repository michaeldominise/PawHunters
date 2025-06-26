using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(menuName = "Addressables/AddressableEnvironmentSettings", fileName = "AddressableEnvironmentSettings")]
    public partial class AddressableEnvironmentSettings : ScriptableObject
    {
        public enum ProfileType { Local, Remote }

        [System.Serializable]
        public class EnvironmentData
        {
            public enum EnvironmentType { Dev, QA, Prod }

            [System.Serializable]
            public class BucketData
            {
                public BuildTarget buildTarget;
                public string id;
            }

            public EnvironmentType environmentType;
            public string id;
            public List<BucketData> buckets;
        }

        public ProfileType profile;
        public EnvironmentData.EnvironmentType buildEnvironment;
        public List<EnvironmentData> environments;
        public bool useOverrideLoadPath;
        [TextArea(3, 10)] public string overrideLoadPath;
    }
}