using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableCharacterData : SaveableDataEntity
    {
        public override AssetReferenceMasterID<EntityMainController> PrefabAssetReference => CharacterEntityOverview.Instance.GetAsset(MasterID);

        public SaveableCharacterData() : base() => masterId = "Character.BlackCat";
    }
}
