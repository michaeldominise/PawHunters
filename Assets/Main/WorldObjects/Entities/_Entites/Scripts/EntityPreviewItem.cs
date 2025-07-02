using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class EntityPreviewItem<T> : PoolItem<T> where T : SaveableDataEntity
    {
        public enum State { Selected, NotSelected }

        [SerializeField] SaveableDataEntity.AssetType assetType = SaveableDataEntity.AssetType.Prefab;
        [SerializeField] Transform assetParent;
        [SerializeField] Button button;
        [SerializeField] GameObject normalBorder;
        [SerializeField] GameObject selectionBorder;
        [SerializeField] List<RarityUI> rarityUIList;
        [SerializeField] SpriteMaskInteraction spriteMaskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; set; } = new(State.NotSelected);

        public EntityMainController Asset { get; set; }
        public Action<EntityPreviewItem<T>> onClick;
        public Action<EntityPreviewItem<T>> onLoaded;

        void Start() => button?.onClick.AddListener(OnClick);
        void OnClick()
        {
            if (Asset == null)
                return;
            SetState(CurrentState.Value == State.Selected ? State.NotSelected : State.Selected);
            onClick?.Invoke(this);
        }

        [Button]
        public void SetState(State state)
        {
            CurrentState.Value = state;
            normalBorder?.SetActive(CurrentState.Value == State.NotSelected);
            selectionBorder?.SetActive(CurrentState.Value == State.Selected);
        }

        public override void Init(int index, T data)
        {
            base.Init(index, data);
            rarityUIList.ForEach(x => x.Init(data.Rarity));
        }

        public override async void Load()
        {
            await data.LoadAssets(assetType);
            if (!data.GetPrefab())
                return;

            Asset = Instantiate(data.GetPrefab(), Vector3.zero, Quaternion.identity, assetParent);
            Asset.transform.localPosition = Vector3.zero;
            Asset.Init(null, data);
            Asset.LayerManager.SetSpritesMaskInteraction(spriteMaskInteraction);
            onLoaded?.Invoke(this);
        }

        public override void Unload()
        {
            if (Asset)
                Destroy(Asset.gameObject);
            if (data != null)
            {
                data.UnloadAssets(assetType);
                data = null;
            }
        }
    }
}
