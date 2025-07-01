using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EquipmentMainController : EntityMainController
    {
        [SerializeField] EquipmentType equipmentType;
        public EquipmentType EquipmentType => equipmentType;
    }
}
