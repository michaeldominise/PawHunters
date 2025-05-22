using System.Collections.Generic;
using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "GameData/LevelData")]
    public class LevelData : ScriptableObject
    {
        public EnvironmentItem environmentItem;
        public List<JourneyData> journeys;
    }
}
