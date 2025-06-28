using System.Linq;
using LabHavenInteractive.PawHunters;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [System.Serializable]
    public class SaveableEquipmentInstanceReference : InstanceReference<SaveableEquipmentData>
    {
        public override SaveableEquipmentData SaveableData => Bag.Instance.equipmentCollection.items.FirstOrDefault(x => x.InstanceData.instanceId == instanceId);
    }
}
