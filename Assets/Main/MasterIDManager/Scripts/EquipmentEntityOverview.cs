using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "EquipmentEntityOverview", menuName = "GameData/Overviews/EquipmentEntityOverview")]
    public class EquipmentEntityOverview : EntityOverview<EquipmentEntityOverview.AssetReferenceMasterID, EquipmentMainController>
    {
        public static EquipmentEntityOverview Instance => MasterIDManager.Instance.equipmentEntityOverview;

        [System.Serializable]
        public class AssetReferenceMasterID : AssetReferenceMasterID<EquipmentMainController>
        {
            [ReadOnly] public ElementType elementType;
            [ReadOnly] public EquipmentType equipmentType;
        }

        public override AssetReferenceMasterID Create(string guid)
        {
            var assetReferenceMasterID = base.Create(guid);
#if UNITY_EDITOR
            assetReferenceMasterID.elementType = assetReferenceMasterID.AssetReference.Element;
            assetReferenceMasterID.equipmentType = assetReferenceMasterID.AssetReference.EquipmentType;
#endif
            return assetReferenceMasterID;
        }
    }
}
