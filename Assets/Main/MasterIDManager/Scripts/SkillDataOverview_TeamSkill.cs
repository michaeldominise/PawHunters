using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "SkillDataOverview_TeamSkill", menuName = "GameData/Overviews/SkillDataOverview_TeamSkill")]
    public class SkillDataOverview_TeamSkill : DataOverview<SkillData>
    {
        public static SkillDataOverview Instance => MasterIDManager.Instance.skillDataOverview;

        protected override bool IsValid(string guid)
        {
#if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab != null && prefab.GetComponentInChildren<SkillData>(true) != null;
#else
            return base.IsValid(guid);
#endif
        }
        protected override string SearchString => $"t:Prefab";
    }
}
