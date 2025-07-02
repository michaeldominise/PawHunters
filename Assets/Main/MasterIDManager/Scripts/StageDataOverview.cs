using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "StageDataOverview", menuName = "GameData/Overviews/StageDataOverview")]
    public class StageDataOverview : DataOverview<StageDataOverview.AssetReferenceMasterID, StageData>
    {
        public static StageDataOverview Instance => MasterIDManager.Instance.stageDataOverview;

        [System.Serializable]
        public class AssetReferenceMasterID : AssetReferenceMasterID<StageData> { }
    }
}
