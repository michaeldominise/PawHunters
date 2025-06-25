using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class InstanceReference<T> where T : SaveableData
    {
        public int instanceId;
        public abstract T SaveableData { get; }
    }
}
