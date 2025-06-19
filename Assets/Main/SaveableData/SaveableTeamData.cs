using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [Serializable]
    public class SaveableTeamData : SaveableData<SaveableCharacterData.AssetType>
    {
        public string teamName; 
        public List<SaveableCharacterData> characters;

        public override List<IAssetReferenceMasterID> GetAssetReference(SaveableCharacterData.AssetType assetType)
        {
            var list = new List<IAssetReferenceMasterID>();
            characters.ForEach(x => list.AddRange(x.GetAssetReference(assetType)));
            return list;
        }
    }
}
