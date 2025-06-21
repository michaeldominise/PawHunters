using System.Collections.Generic;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public partial class UserData : SaveableData
    {
        [System.Serializable]
        public class BattleData
        {
            public AssetReferenceMasterID<StageData> huntStageData;
        }
    }
}
