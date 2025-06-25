using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class HunterPreviewItem : PoolItem<SaveableCharacterData>
    {
        [SerializeField] SaveableCharacterData.AssetType assetType = SaveableCharacterData.AssetType.character;
        [SerializeField] Transform assetParent;
        EntityMainController asset;

        protected override async void Load()
        {
            await data.LoadAssets(assetType);
            if (!data.GetPrefab())
                return;

            asset = Instantiate(data.GetPrefab(), Vector3.zero, Quaternion.identity, assetParent);
            asset.transform.localPosition = Vector3.zero;
            asset.LayerManager.SetSpritesMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
        }

        protected override void Unload()
        {
            if(asset)
                Destroy(asset.gameObject);
            if(data != null)
                data.UnloadAssets(assetType);
        }
    }
}
