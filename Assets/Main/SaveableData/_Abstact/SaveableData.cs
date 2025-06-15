using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [Serializable]
    public abstract class SaveableData
    {
        static List<Action> OnExecuteUpdateList = new();
        public static void Execute()
        {
            OnExecuteUpdateList.ForEach(x => x?.Invoke());
            OnExecuteUpdateList.Clear();
        }

        event Action OnValueChange;
        void OnValueChangeMethod() => OnValueChange?.Invoke();

        public void UnregisterOnValueChange(Action action) => OnValueChange -= action;
        public void RegisterOnValueChange(Action action)
        {
            if (OnValueChange?.GetInvocationList().Contains(action) ?? false)
                return;
            OnValueChange += action;
        }

        [Button]
        public void SetDirty()
        {
            if (OnExecuteUpdateList.Contains(OnValueChangeMethod))
                return;
            OnExecuteUpdateList.Add(OnValueChangeMethod);
        }

        public static T Initialize<T>(ref T oldData, T newData, Action onValueChange) where T : SaveableData
        {
            oldData?.UnregisterOnValueChange(onValueChange);
            newData?.RegisterOnValueChange(onValueChange);
            oldData = newData;
            return newData;
        }

        public virtual List<IAssetReferenceMasterID> GetAssetReference() => new();

        public virtual async Task LoadAssets()
        {
            var loadTask = GetAssetReference().Select(x => x.Load());
            if (loadTask.Count() > 0)
                await Task.WhenAll(loadTask);
        }

        public void UnloadAssets() => GetAssetReference().ForEach(x => x.Unload());
    }
}
