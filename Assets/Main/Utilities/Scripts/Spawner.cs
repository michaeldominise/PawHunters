using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace PawHunters
{
    public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
    {
        public event Action<T> OnSpawned;

        [SerializeField] protected Transform spawnParent;

        protected List<KeyValuePair<int, T>> spawnedList = new();

        public Transform SpawnParent => spawnParent;

        protected virtual Vector3 GetSpawnPoint() => Vector3.zero;

        public virtual T Spawn(T prefab, Func<T, bool> condition = null, Action<T> init = null)
        {
            var keyPairItem = spawnedList.FirstOrDefault(x => x.Key == prefab.GetInstanceID()  && !x.Value.gameObject.activeSelf && (condition?.Invoke(x.Value) ?? true));
            var item = keyPairItem.Value;
            if (!item)
                item = Instantiate(prefab);
            else
                spawnedList.Remove(keyPairItem);

            item.gameObject.SetActive(true);
            item.transform.localPosition = GetSpawnPoint();
            item.transform.rotation = Quaternion.identity;
            item.transform.SetParent(spawnParent);
            spawnedList.Add(new(prefab.GetInstanceID(), item));
            init?.Invoke(item);
            OnSpawned?.Invoke(item);
            return item;
        }

        public void Despawn(T spawnedItem, float setInactiveDelay = 0) => StartCoroutine(_Despawn(spawnedItem, setInactiveDelay));
        IEnumerator _Despawn(T spawnedItem, float setInactiveDelay)
        {
            spawnedItem.transform.SetParent(transform);
            yield return new WaitForSeconds(setInactiveDelay);
            spawnedItem.gameObject.SetActive(false);
        }

        public virtual void Clear() => spawnedList.ForEach(x =>
        {
            x.Value?.transform.SetParent(spawnParent);
            x.Value?.gameObject.SetActive(false);
        });
    }
}
