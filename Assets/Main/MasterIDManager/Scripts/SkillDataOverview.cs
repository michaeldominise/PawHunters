using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "SkillDataOverview", menuName = "GameData/Overviews/SkillDataOverview")]
    public class SkillDataOverview : ScriptableObject
    {
        public static SkillDataOverview Instance => MasterIDManager.Instance.skillDataOverview;

        public SkillData[] skillDataCommonList;
    }
}
