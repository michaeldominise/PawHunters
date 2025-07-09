using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EntityAssetManager : MonoBehaviour
    {
        [SerializeField] Transform assetParentCharacter;
        [SerializeField] Transform assetParentWeapon;
        [SerializeField] OverlayUI_Rarity overlayUI;

        SaveableDataEntity data;
        EntityMainController asset;

        public void Init(SaveableDataEntity data)
        {
            if (asset)
                Destroy(asset.gameObject);
            this.data = data;
            overlayUI.Init(data.Rarity);
            asset = Instantiate(data.GetPrefab(), data is SaveableCharacterData ? assetParentCharacter : assetParentWeapon);
            asset.transform.localPosition = Vector3.zero;
        }
    }
}
