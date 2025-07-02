using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "SkillDataOverview", menuName = "GameData/Overviews/SkillDataOverview")]
    public class SkillDataOverview : DataOverview<SkillDataOverview.AssetReferenceMasterID, SkillData>
    {
        public static SkillDataOverview Instance => MasterIDManager.Instance.skillDataOverview;

        [System.Serializable]
        public class AssetReferenceMasterID : AssetReferenceMasterID<SkillData> { }

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
