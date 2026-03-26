using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
    {
        public event Action<T> OnSpawned;

        [SerializeField] protected Transform spawnParent;

        protected List<KeyValuePair<int, T>> spawnedList = new();
        public List<T> activeList = new();

        public Transform SpawnParent => spawnParent;

        protected virtual Vector3 GetSpawnPoint() => Vector3.zero;

        public virtual T Spawn(T prefab, Func<T, bool> condition = null, Action<T> init = null)
        {
            var keyPairItem = spawnedList.FirstOrDefault(x => x.Key == prefab.GetInstanceID()  && !x.Value.gameObject.activeSelf && (condition?.Invoke(x.Value) ?? true));
            var item = keyPairItem.Value;
            if (!item)
                item = Instantiate(prefab, spawnParent);
            else
                spawnedList.Remove(keyPairItem);

            item.gameObject.SetActive(true);
            item.transform.localPosition = GetSpawnPoint();
            spawnedList.Add(new(prefab.GetInstanceID(), item));
            activeList.Add(item);
            init?.Invoke(item);
            OnSpawned?.Invoke(item);
            return item;
        }

        public async void Despawn(T spawnedItem, float setInactiveDelay = 0)
        {
            activeList.Remove(spawnedItem);
            if(transform.gameObject)
                spawnedItem.transform.SetParent(transform);
            await Task.Delay((int)(setInactiveDelay * 1000));
            spawnedItem.gameObject.SetActive(false);
        }

        public virtual void Clear()
        {
            activeList.Clear();
            spawnedList.ForEach(x =>
            {
                x.Value.transform.SetParent(spawnParent);
                x.Value.gameObject.SetActive(false);
            });
        }
    }
}
