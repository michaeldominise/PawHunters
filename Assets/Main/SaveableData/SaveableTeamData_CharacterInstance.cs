using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableTeamData_CharacterInstance : SaveableTeamData
    {
        [SerializeField] List<SaveableCharacterInstanceData> characters;

        public override List<SaveableCharacterData> Characters => characters.Select(x => x.SaveableData).ToList();
    }
}
