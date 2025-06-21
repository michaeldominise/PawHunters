using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "MasterIDManager", menuName = "GameData/Overviews/MasterIDManager")]
    public class MasterIDManager : ScriptableObject
    {
        public static MasterIDManager Instance => AppManager.Instance.MasterIDManager;

        public SkillDataOverview skillDataOverview;
        public StageDataOverview stageDataOverview;
        public SkillDataOverview_TeamSkill skillDataOverview_TeamSkill;
        public EntityOverview entityOverview;
    }
}
