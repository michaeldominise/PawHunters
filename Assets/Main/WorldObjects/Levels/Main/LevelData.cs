using System.Collections.Generic;
using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "GameData/LevelData")]
    public class LevelData : ScriptableObject
    {
        public string title;
        public int maxRound = -1;
        public EnvironmentItem environmentItem;
        public List<JourneyData> journeys;
    }
}
