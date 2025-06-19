using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LabHaven.PawHunters
{
    [System.Serializable]
    public class AssetReferenceScene : AssetReference
    {
        public AssetReferenceScene(string guid) : base(guid) { }
        public override bool ValidateAsset(string path) => path.EndsWith(".unity");
    }
}
