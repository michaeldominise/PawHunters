using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class SaveableTeamData : SaveableData<SaveableCharacterData.AssetType>
    {
        public string teamName = "Sub Pact";
        public abstract List<SaveableCharacterData> Characters { get; }

        public override List<IAssetReferenceMasterID> GetAssetReference(SaveableCharacterData.AssetType assetType)
        {
            var list = new List<IAssetReferenceMasterID>();
            Characters.Where(x => x != null).ToList().ForEach(x => list.AddRange(x.GetAssetReference(assetType)));
            return list;
        }

        public SaveableTeamData() { }
        public SaveableTeamData(string teamName) => this.teamName = teamName;
    }
}
