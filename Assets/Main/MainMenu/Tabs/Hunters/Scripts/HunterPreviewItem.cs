using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class HunterPreviewItem : PoolItem<SaveableCharacterData>
    {
        public enum State { Selected, NotSelected }

        [SerializeField] SaveableCharacterData.AssetType assetType = SaveableCharacterData.AssetType.character;
        [SerializeField] Transform assetParent;
        [SerializeField] Button button;
        [SerializeField] GameObject selectionBorder;
        [SerializeField] List<RarityUI> rarityUIList;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; set; } = new(State.NotSelected);

        public EntityMainController Asset { get; set; }
        public Action<HunterPreviewItem> onClick;
        public Action<HunterPreviewItem> onLoaded;

        void Start() => button.onClick.AddListener(OnClick);
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
            selectionBorder.SetActive(CurrentState.Value == State.Selected);
        }

        public override void Init(int index, SaveableCharacterData data)
        {
            base.Init(index, data);
            rarityUIList.ForEach(x => x.Init(data.Rarity));
        }

        protected override async void Load()
        {
            await data.LoadAssets(assetType);
            if (!data.GetPrefab())
                return;

            Asset = Instantiate(data.GetPrefab(), Vector3.zero, Quaternion.identity, assetParent);
            Asset.transform.localPosition = Vector3.zero;
            Asset.Init(null, data);
            Asset.LayerManager.SetSpritesMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
            onLoaded?.Invoke(this);
        }

        protected override void Unload()
        {
            if(Asset)
                Destroy(Asset.gameObject);
            if(data != null)
                data.UnloadAssets(assetType);
        }
    }
}
