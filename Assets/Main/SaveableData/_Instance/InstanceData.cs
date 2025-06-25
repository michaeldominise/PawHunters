using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class InstanceData
    {
        static int _InstanceCount;
        static int InstanceCount
        {
            get
            {
                var value = _InstanceCount;
                _InstanceCount++;
                return value;
            }
        }

        public int instanceId = InstanceCount;
        public string dateCreatedString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        public string dateOwnedString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }
}
