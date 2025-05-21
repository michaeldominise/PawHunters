using System.Collections.Generic;
using UnityEngine;

namespace PawHunters
{
    [System.Serializable]
    public class SaveableTeamData : SaveableData
    {
        public string teamName; 
        public List<SaveableCharacterData> characters;
    }
}
