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
    public class AssetReferenceMasterID<T> : IAssetReferenceMasterID<T>, IAssetReferenceMasterID, IAssetReference where T : Object
    {
#if UNITY_EDITOR
        AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;
#endif

        [SerializeField, HideInInspector] string masterID;
        [SerializeField, HideInInspector] AssetReferenceT<T> assetReference;

        [ShowInInspector, ReadOnly, HideLabel] public string MasterID => masterID;
        public int UsageCount { get; set; }
        public T Asset { get; private set; }
        public string GUID
        {
            get => assetReference.AssetGUID;
            set
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(value);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                AssetReference = asset;
            }
        }

        Object IAssetReferenceMasterID.Asset => Asset;

#if UNITY_EDITOR
        [ShowInInspector, HideLabel] public T AssetReference
        {
            get => assetReference.editorAsset;
            private set
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

        public static explicit operator AssetReferenceMasterID<T>(T asset) => Create(asset);
        public static AssetReferenceMasterID<T> Create(T asset) => new() { AssetReference = asset };

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
            {
                AppManager.AddAssetReferences(this, Asset);
                return Asset;
            }

            var operation = assetReference.IsDone ? assetReference.LoadAssetAsync<Object>() : assetReference.OperationHandle;
            await operation.Task;

            if (!assetReference.IsDone)
                return null;
            else if (operation.Result is T)
                Asset = operation.Result as T;
            else if (operation.Result is GameObject)
                Asset = (operation.Result as GameObject).GetComponent<T>();
            else
                return null;

            AppManager.AddAssetReferences(this, Asset);
            return Asset;
        }

        public async void Unload()
        {
            UsageCount--;
#if UNITY_EDITOR
            if(EditorApplication.isPlayingOrWillChangePlaymode)
#endif
                await Task.Yield();
            if (UsageCount != 0)
                return;
            Asset = null;
            AppManager.RemoveAssetReferences(this);
            assetReference.ReleaseAsset();
        }
    }

    public interface IAssetReference
    {
        Task Load();
        void Unload();
    }

    public interface IAssetReferenceMasterID : IAssetReference
    {
        string MasterID { get; }
        int UsageCount { get; set; }
        Object Asset { get; }
    }

    public interface IAssetReferenceMasterID<T> : IAssetReferenceMasterID where T : Object
    {
        Task<T> LoadAsset();
    }
}
