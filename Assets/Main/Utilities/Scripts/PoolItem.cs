using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class PoolItem<T> : MonoBehaviour where T : SaveableData
    {
        [SerializeField] int index;
        [ShowInInspector, ReadOnly] protected T data;

        public T Data => data;

        public virtual void Refresh() => Init(index, data);
        public virtual void Init(int index, T data)
        {
            if(this.data != null)
                Unload();
            this.index = index;
            this.data = SaveableData.Initialize(ref this.data, data, Refresh);
            Load();
        }

        public virtual void Load() { }

        public virtual void Unload() { }

        private void OnDestroy() => Unload();
    }
}
