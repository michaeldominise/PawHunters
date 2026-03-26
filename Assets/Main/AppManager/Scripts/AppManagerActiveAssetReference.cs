using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public partial class AppManager
    {
        [ShowInInspector, HideReferenceObjectPicker, BoxGroup("Active Asset References", ShowLabel = false)]
        static List<AssetReferenceMasterIDProfiler> ActiveAssetReferences = new();

        [System.Serializable]
        public class AssetReferenceMasterIDProfiler
        {
            [ShowInInspector, ReadOnly] public string MasterID => iAssetReference.MasterID;
            [ShowInInspector, ReadOnly] public int UsageCount => iAssetReference.UsageCount;
            [ShowInInspector, ReadOnly] public Object Asset { get; set; }

            public IAssetReferenceMasterID iAssetReference;

            public AssetReferenceMasterIDProfiler(IAssetReferenceMasterID iAssetReference, Object asset)
            {
                Asset = asset;
                this.iAssetReference = iAssetReference;
            }
        }

        public static void AddAssetReferences(IAssetReferenceMasterID iAssetReference, Object asset)
        {
            if (ActiveAssetReferences.Exists(x => x.iAssetReference == iAssetReference))
                return;
            ActiveAssetReferences.Add(new(iAssetReference, asset));
        }
        public static void RemoveAssetReferences(IAssetReferenceMasterID iAssetReference) => ActiveAssetReferences.Remove(ActiveAssetReferences.FirstOrDefault(x => x.iAssetReference == iAssetReference));

        //private void OnDestroy() => UnloadActiveAssets();
        [Button, BoxGroup("Active Asset References", ShowLabel = false)]
        private void UnloadActiveAssets()
        {
            var references = new List<AssetReferenceMasterIDProfiler>(ActiveAssetReferences);
            foreach (var activeAssetReference in references)
            {
                activeAssetReference.iAssetReference.UsageCount = 1;
                activeAssetReference.iAssetReference.Unload();
            }
            ActiveAssetReferences.Clear();
        }
    }
}
