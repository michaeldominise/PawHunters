using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableEquipmentData : SaveableDataEntity
    {
        public override AssetReferenceMasterID<EntityMainController> PrefabAssetReference => EquipmentEntityOverview.Instance.GetAsset(MasterID);

        public SaveableEquipmentData() : base() => masterId = "Equipment.Test";
    }
}
