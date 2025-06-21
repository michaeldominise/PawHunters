using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace LabHavenInteractive.PawHunters
{
    [System.Serializable]
    [InlineProperty]
    public class AssetReferenceMasterID<T> : IAssetReferenceMasterID, IAssetReferenceMasterID<T>, IAssetReference where T : Object
    {
#if UNITY_EDITOR
        AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;
#endif

        [SerializeField, HideInInspector] string masterID;
        [SerializeField, HideInInspector] AssetReferenceT<T> assetReference;

        [ShowInInspector, ReadOnly, HideLabel] public string MasterID => masterID;
        public int UsageCount { get; set; }
        public T Asset { get; private set; }

#if UNITY_EDITOR
        [ShowInInspector, HideLabel] T AssetReference
        {
            get => assetReference.editorAsset;
            set
            {
                if (value == null)
                {
                    assetReference = null;
                    masterID = string.Empty;
                    return;
                }

                var guid = SetAsAddressable(value);
                assetReference = new(guid);
                masterID = $"{value.name}";
                EditorUtility.SetDirty(Selection.activeObject);
            }
        }

        public static explicit operator AssetReferenceMasterID<T>(T asset) => new(asset);
        public AssetReferenceMasterID(T asset) => AssetReference = asset;
        public AssetReferenceMasterID(string guid)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            AssetReference = asset;
        }

        string SetAsAddressable(T asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset);
            var guid = AssetDatabase.GUIDFromAssetPath(assetPath).ToString();
            if (Settings.FindAssetEntry(guid) == null)
            {
                var entry = Settings.CreateOrMoveEntry(guid, Settings.DefaultGroup);
                Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
                EditorUtility.SetDirty(asset);
            }
            return guid;
        }
#endif

        public async Task Load() => await LoadAsset();
        public async Task<T> LoadAsset()
        {
            UsageCount++;
            if (Asset)
                return Asset;

            var operation = assetReference.IsDone ? assetReference.LoadAssetAsync<Object>() : assetReference.OperationHandle;
            await operation.Task;

            if (operation.Result is T)
                Asset = operation.Result as T;
            else if (operation.Result is GameObject)
                Asset = (operation.Result as GameObject).GetComponent<T>();
            else
                return null;

            if (UsageCount == 1)
                AppManager.AddAssetReferences(this, Asset);
            return Asset;
        }

        public void Unload()
        {
            if (UsageCount == 0)
                return;
            UsageCount--;
            Asset = null;
            AppManager.RemoveAssetReferences(this);
            assetReference.ReleaseAsset();
        }
    }

    public interface IAssetReference
    {
        public Task Load();
        public void Unload();
    }

    public interface IAssetReferenceMasterID
    {
        public string MasterID { get; }
        public int UsageCount { get; set; }
        public Task Load();
        public void Unload();
    }

    public interface IAssetReferenceMasterID<T> where T : Object
    {
        public string MasterID { get; }
        public int UsageCount { get; set; }
        public Task<T> LoadAsset();
        public void Unload();
    }
}
