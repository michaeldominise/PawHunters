using Sirenix.OdinInspector;
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
        public CharacterEntityOverview characterEntityOverview;
        public EquipmentEntityOverview equipmentEntityOverview;

        [Button]
        public void Refresh()
        {
            skillDataOverview.Refresh();
            stageDataOverview.Refresh();
            skillDataOverview_TeamSkill.Refresh();
            characterEntityOverview.Refresh();
            equipmentEntityOverview.Refresh();
        }
    }
}
