using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "MasterIDManager", menuName = "GameData/Overviews/MasterIDManager")]
    public class MasterIDManager : ScriptableObject
    {
        public static MasterIDManager Instance => GameManager.Instance.MasterIDManager;

        public SkillDataOverview skillDataOverview;
    }
}
