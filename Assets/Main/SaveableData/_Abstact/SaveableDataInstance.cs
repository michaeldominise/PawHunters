using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class InstanceData<T> where T : SaveableData
    {
        public string instanceId;
        public abstract T SaveableData { get; }
    }
}
