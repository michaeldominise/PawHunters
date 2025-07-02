using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class EntityOverview<TAssetReferenceMasterID, TComponent> : DataOverview<TAssetReferenceMasterID, TComponent>
        where TAssetReferenceMasterID : AssetReferenceMasterID<TComponent>, new()
        where TComponent : Component
    {
        protected override bool IsValid(string guid)
        {
#if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab != null && prefab.GetComponentInChildren<TComponent>(true) != null;
#else
            return base.IsValid(guid);
#endif
        }
        protected override string SearchString => $"t:Prefab";
    }
}
