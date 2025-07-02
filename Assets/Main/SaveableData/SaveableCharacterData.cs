using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableCharacterData : SaveableDataEntity
    {
        public SaveableCharacterData() : base() => masterId = "Character.BlackCat";

        public override IAssetReferenceMasterID PrefabAssetReference => CharacterEntityOverview.Instance.GetAsset(masterId);
    }
}
