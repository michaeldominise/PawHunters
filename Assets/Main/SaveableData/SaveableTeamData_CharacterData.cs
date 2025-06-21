using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableTeamData_CharacterData : SaveableTeamData
    {
        [SerializeField] List<SaveableCharacterData> characters;

        public override List<SaveableCharacterData> Characters => characters;
    }
}
