using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "EquipmentEntityOverview", menuName = "GameData/Overviews/EquipmentEntityOverview")]
    public class EquipmentEntityOverview : EntityOverview<EquipmentMainController>
    {
        public static EquipmentEntityOverview Instance => MasterIDManager.Instance.equipmentEntityOverview;
    }
}
