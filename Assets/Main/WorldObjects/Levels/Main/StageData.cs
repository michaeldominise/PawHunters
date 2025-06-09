using System.Collections.Generic;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "GameData/LevelData")]
    public class StageData : ScriptableObject
    {
        public string title;
        public int maxRound = -1;
        public EnvironmentItem environmentItem;
        public List<JourneyData> journeys;
    }
}
