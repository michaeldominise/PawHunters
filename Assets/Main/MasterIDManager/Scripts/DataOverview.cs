using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class DataOverview<TAssetReferenceMasterID, T> : ScriptableObject, IRefresh
        where TAssetReferenceMasterID : AssetReferenceMasterID<T>, new()
        where T : Object
    {
        public List<TAssetReferenceMasterID> dataList;

        [SerializeField, HideInInspector] string[] folderPaths;
#if UNITY_EDITOR
        [SerializeField, HideInInspector] DefaultAsset[] folderAssets;
        [ShowInInspector]
        DefaultAsset[] FolderAssets
        {
            get => folderAssets;
            set
            {
                folderAssets = value;
                folderPaths = folderAssets.Select(x => AssetDatabase.GetAssetPath(x)).ToArray();
            }
        }
#endif

        public IAssetReferenceMasterID<T> GetAsset(string masterID)
        {
            var assetReference = dataList.FirstOrDefault(x => x.MasterID == masterID);
            if (assetReference == null)
                Debug.LogError($"{masterID} is not found in the {this.name}");
            return assetReference;
        }

        protected virtual bool IsValid(string guid) => true;
        protected virtual string SearchString => $"t:{typeof(T).Name}";

        [Button]
        public void Refresh()
        {
#if UNITY_EDITOR
            dataList.Clear();

            var guids = AssetDatabase.FindAssets(SearchString, folderPaths);
            foreach (string guid in guids)
            {
                if (IsValid(guid))
                    dataList.Add(Create(guid));
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }

        public virtual TAssetReferenceMasterID Create(string guid) => new() { GUID = guid };
    }

    public interface IRefresh
    {
        public void Refresh();
    }
}
