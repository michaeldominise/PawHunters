using System.Collections.Generic;
using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "GameData/LevelData")]
    public class LevelData : ScriptableObject
    {
        public EnvironmentItem environmentItem;
        public int maxRound = -1;
        public List<JourneyData> journeys;
    }
}
