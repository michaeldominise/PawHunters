using System;
using System.Collections;
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
        [SerializeField] protected List<T1> spawnedList;

        public Transform SpawnParent => spawnParent;

        protected virtual Vector3 GetSpawnPoint() => Vector3.zero;

        public virtual T1 Spawn(T1 prefab, Func<T1, bool> condition = null, Action<T1> init = null)
        {
            var item = spawnedList.FirstOrDefault(x => x.GetInstanceID() == prefab.GetInstanceID()  && !x.gameObject.activeInHierarchy && (condition?.Invoke(x) ?? true));
            if (!item)
                item = Instantiate(prefab);
            else
                spawnedList.Remove(item);

            item.gameObject.SetActive(true);
            item.transform.localPosition = GetSpawnPoint();
            item.transform.rotation = Quaternion.identity;
            item.transform.SetParent(spawnParent);
            spawnedList.Add(item);
            init?.Invoke(item);
            OnSpawned?.Invoke(item);
            return item;
        }

        public void Despawn(T1 spawnedItem, float setInactiveDelay = 0) => StartCoroutine(_Despawn(spawnedItem, setInactiveDelay));
        IEnumerator _Despawn(T1 spawnedItem, float setInactiveDelay)
        {
            spawnedItem.transform.SetParent(transform);
            yield return new WaitForSeconds(setInactiveDelay);
            spawnedItem.gameObject.SetActive(false);
        }

        public virtual void Clear() => spawnedList.ForEach(x =>
        {
            x.transform.SetParent(spawnParent);
            x.gameObject.SetActive(false);
        });
    }
}
