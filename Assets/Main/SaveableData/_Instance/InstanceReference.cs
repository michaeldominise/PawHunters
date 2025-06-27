using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class InstanceReference<T> where T : SaveableData
    {
        public long instanceId = -1;
        public abstract T SaveableData { get; }
    }
}
