using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableEquipmentData : SaveableDataEntity
    {
        public SaveableEquipmentData() : base() => masterId = "Equipment.Test";

        public override IAssetReferenceMasterID PrefabAssetReference => EquipmentEntityOverview.Instance.GetAsset(MasterID);
        public override ElementType ElementType => (PrefabAssetReference as EquipmentEntityOverview.AssetReferenceMasterID).elementType;
    }
}
