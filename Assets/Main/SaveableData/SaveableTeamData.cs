using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [System.Serializable]
    public class SaveableTeamData : SaveableData
    {
        public string teamName; 
        public List<SaveableCharacterData> characters;

        public override List<IAssetReferenceMasterID> GetAssetReference()
        {
            var list = new List<IAssetReferenceMasterID>();
            characters.ForEach(x => list.AddRange(x.GetAssetReference()));
            return list;
        }
    }
}
