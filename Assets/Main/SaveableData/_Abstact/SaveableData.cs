using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class SaveableData : IInstanceData
    {
        [SerializeField] InstanceData instanceData;
        public InstanceData InstanceData => instanceData;

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
            return newData;
        }
    }

    public abstract class SaveableData<AssetType> : SaveableData where AssetType : Enum
    {
        public virtual List<IAssetReferenceMasterID> GetAssetReference(AssetType assetType) => new();

        public virtual async Task LoadAssets(AssetType assetType)
        {
            var loadTask = GetAssetReference(assetType).Select(x => x.Load());
            await Task.WhenAll(loadTask);
        }

        public void UnloadAssets(AssetType assetType) => GetAssetReference(assetType).ForEach(x => x.Unload());
    }

    public interface IMasterId
    {
        public string MasterId { get; }
    }
}
