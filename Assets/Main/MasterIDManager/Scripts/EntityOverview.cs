using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "EntityOverview", menuName = "GameData/Overviews/EntityOverview")]
    public class EntityOverview : DataOverview<EntityMainController>
    {
        public static EntityOverview Instance => MasterIDManager.Instance.entityOverview;

        protected override bool IsValid(string guid)
        {
#if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab != null && prefab.GetComponentInChildren<EntityMainController>(true) != null;
#else
            return base.IsValid(guid);
#endif
        }
        protected override string SearchString => $"t:Prefab";
    }
}
