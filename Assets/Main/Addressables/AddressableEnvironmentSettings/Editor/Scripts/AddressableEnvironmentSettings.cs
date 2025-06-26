using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(menuName = "Addressables/AddressableEnvironmentSettings", fileName = "AddressableEnvironmentSettings")]
    public partial class AddressableEnvironmentSettings : ScriptableObject
    {
        public enum ProfileType { Local, Remote }
        const string EnvironmentNameKey = "EnvironmentName";

        [System.Serializable]
        public class EnvironmentData
        {
            public enum EnvironmentType { Dev, QA, Production }

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

        [PropertyOrder(-2)] public ProfileType profile;
        [SerializeField, HideInInspector] EnvironmentData.EnvironmentType buildEnvironment;
        public List<EnvironmentData> environments;
        public bool useOverrideLoadPath;
        [TextArea(3, 10)] public string overrideLoadPath;

        [ShowInInspector, PropertyOrder(-1)]
        public EnvironmentData.EnvironmentType BuildEnvironment
        {
            get => buildEnvironment;
            set => SetBuildEnvironment(value);
        }

        public void SetBuildEnvironment(EnvironmentData.EnvironmentType environmentType)
        {
            buildEnvironment = environmentType;
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            string activeProfileId = settings.activeProfileId;
            settings.profileSettings.SetValue(activeProfileId, EnvironmentNameKey, environmentType.ToString().ToLower());

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }
    }
}