using System.Collections.Generic;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [System.Serializable]
    public class SaveableTeamData : SaveableData
    {
        public string teamName; 
        public List<SaveableCharacterData> characters;
    }
}
