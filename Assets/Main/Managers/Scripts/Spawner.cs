using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace PawHunters
{
    public class Spawner<T1> : MonoBehaviour where T1 : MonoBehaviour
    {
        public event Action<T1> OnSpawned;

        [SerializeField] protected Transform spawnParent;
        [SerializeField] protected List<T1> spawnedlist;

        protected virtual Vector3 GetSpawnPoint() => Vector3.zero;

        public virtual T1 Spawn(T1 prefab, Func<T1, bool> condition = null, Action<T1> init = null)
        {
            var item = spawnedlist.FirstOrDefault(x => (condition?.Invoke(x) ?? true)  && !x.gameObject.activeInHierarchy);
            if (!item)
            {
                item = Instantiate(prefab, GetSpawnPoint(), Quaternion.identity, spawnParent);
                spawnedlist.Add(item);
            }

            init?.Invoke(item);
            OnSpawned?.Invoke(item);
            return item;
        }
    }
}
