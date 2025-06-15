using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "StageDataOverview", menuName = "GameData/Overviews/StageDataOverview")]
    public class StageDataOverview : DataOverview<StageData>
    {
        public static StageDataOverview Instance => MasterIDManager.Instance.stageDataOverview;
    }
}
