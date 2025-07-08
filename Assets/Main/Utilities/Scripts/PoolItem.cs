using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class PoolItem<T> : MonoBehaviour where T : SaveableData
    {
        [SerializeField] int index;
        [ShowInInspector, ReadOnly] protected T data;

        public T Data => data;

        public virtual void Refresh() => _ = Init(index, data);
        public virtual async Task Init(int index, T data)
        {
            if(this.data != null)
                Unload();
            this.index = index;
            this.data = SaveableData.Initialize(this.data, data, Refresh);
            await Load();
        }

        public virtual async Task Load() => await Task.Yield();

        public virtual void Unload() { }

        private void OnDestroy() => Unload();
    }
}
