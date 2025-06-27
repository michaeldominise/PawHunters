using System.Linq;
using LabHavenInteractive.PawHunters;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [System.Serializable]
    public class SaveableCharacterInstanceReference : InstanceReference<SaveableCharacterData>
    {
        public override SaveableCharacterData SaveableData => Bag.Instance.hunterCollection.items.FirstOrDefault(x => x.InstanceData.instanceId == instanceId);
    }
}
