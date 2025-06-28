using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableTeamData_CharacterInstance : SaveableTeamData
    {
        [FormerlySerializedAs("characters")] public List<SaveableCharacterInstanceReference> characterInstanceList = new() { new(), new(), new() };
        [FormerlySerializedAs("characters")] public List<SaveableEquipmentInstanceReference> equipmentInstanceList = new() { new(), new(), new(), new(), new(), new(), new() };

        public override List<SaveableCharacterData> Characters => characterInstanceList.Select(x => x.SaveableData).ToList();
        public override List<SaveableEquipmentData> Equipments => equipmentInstanceList.Select(x => x.SaveableData).ToList();

        public SaveableTeamData_CharacterInstance() : base() { }
        public SaveableTeamData_CharacterInstance(string teamName) : base(teamName) { }
    }
}