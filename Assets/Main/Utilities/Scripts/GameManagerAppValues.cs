using UnityEngine;

namespace LabHaven.PawHunters
{
    public partial class GameManager
    {
        public AppValues appValues;

        [System.Serializable]
        public class AppValues
        {
            public AssetReferenceMasterID<StageData> stageData;
            public SaveableTeamData playerTeamData;
        }
    }
}
