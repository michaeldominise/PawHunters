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

        public long instanceId = DateTimeOffset.Now.ToUnixTimeMilliseconds() + InstanceCount;
        public string dateCreatedString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        public string dateOwnedString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    public interface IInstanceData
    {
        public InstanceData InstanceData { get; }
    }
}
