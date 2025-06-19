using UnityEngine;

namespace LabHaven.PawHunters
{
    public partial class AppManager
    {
        public UserData userData;

        [System.Serializable]
        public class UserData : SaveableData
        {
            [System.Serializable]
            public class BattleData
            {
                public AssetReferenceMasterID<StageData> huntStageData;
            }

            public BattleData battleData;
            public SaveableTeamData playerTeamData;
        }
    }
}
