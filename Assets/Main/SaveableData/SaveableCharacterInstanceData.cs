using System.Linq;
using LabHavenInteractive.PawHunters;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [System.Serializable]
    public class SaveableCharacterInstanceData : InstanceData<SaveableCharacterData>
    {
        public override SaveableCharacterData SaveableData => Bag.Instance.hunterCollection.items.FirstOrDefault(x => x.instanceID == instanceId);
    }
}
