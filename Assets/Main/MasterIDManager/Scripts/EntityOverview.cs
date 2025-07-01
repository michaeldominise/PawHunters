using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class EntityOverview<EntityMainControllerType> : DataOverview<EntityMainController>
        where EntityMainControllerType : EntityMainController
    {
        protected override bool IsValid(string guid)
        {
#if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab != null && prefab.GetComponentInChildren<EntityMainControllerType>(true) != null;
#else
            return base.IsValid(guid);
#endif
        }
        protected override string SearchString => $"t:Prefab";
    }
}
