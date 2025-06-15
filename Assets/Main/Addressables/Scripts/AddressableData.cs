using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LabHaven.PawHunters
{
    [System.Serializable]
    public abstract class AddressableData<AssetType> where AssetType : System.Enum
    {
        public abstract List<IAssetReferenceMasterID> GetAssetReferences(AssetType assetType);

        public virtual async Task LoadAssets(AssetType assetType)
        {
            var loadTask = GetAssetReferences(assetType).Select(x => x.Load());
            if (loadTask.Count() > 0)
                await Task.WhenAll(loadTask);
        }

        public virtual void UnloadAssets(AssetType assetType) => GetAssetReferences(assetType).ForEach(x => x.Unload());
    }
}
