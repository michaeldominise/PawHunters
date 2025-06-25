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
        [ShowInInspector, ReadOnly] StateController<State> CurrentState { get; set; } = new(State.NotSelected);
        EntityMainController asset;

        private void Start() => button.onClick.AddListener(OnClick);

        public void OnClick()
        {
            CurrentState.Value = CurrentState.Value == State.Selected ? State.NotSelected : State.Selected;
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

            asset = Instantiate(data.GetPrefab(), Vector3.zero, Quaternion.identity, assetParent);
            asset.transform.localPosition = Vector3.zero;
            asset.LayerManager.SetSpritesMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
        }

        protected override void Unload()
        {
            if(asset)
                Destroy(asset.gameObject);
            if(data != null)
                data.UnloadAssets(assetType);
        }
    }
}
