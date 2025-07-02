using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "SkillDataOverview_TeamSkill", menuName = "GameData/Overviews/SkillDataOverview_TeamSkill")]
    public class SkillDataOverview_TeamSkill : SkillDataOverview
    {
        public static new SkillDataOverview Instance => MasterIDManager.Instance.skillDataOverview_TeamSkill;
    }
}
