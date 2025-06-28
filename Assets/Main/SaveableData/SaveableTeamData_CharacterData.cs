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
        [SerializeField] List<SaveableEquipmentData> equipments;

        public override List<SaveableCharacterData> Characters => characters;
        public override List<SaveableEquipmentData> Equipments => equipments;
    }
}
