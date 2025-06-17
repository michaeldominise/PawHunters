using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LabHaven.PawHunters
{
    public partial class AppManager
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
