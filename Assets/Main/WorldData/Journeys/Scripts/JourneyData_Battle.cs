using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_Battle", menuName = "GameData/JourneyData/Battle")]
    public class JourneyData_Battle : JourneyData
    {
        public SaveableTeamData teamData;

        public override void Init()
        {
            JourneyButtons.Instance.Init(response => Execute(), buttonLabel);
        }

        public override void Execute() => TeamManager_GameEnemy.Instance.SpawnEnemy(teamData);   
    }
}
