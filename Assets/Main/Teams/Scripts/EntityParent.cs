using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EntityParent : MonoBehaviour
    {
        public EntityMainController entityMainController;
        public TeamManager teamManager;
        [SerializeField] Transform container;
        [SerializeField] SaveableDataEntity.AssetType assetType;

        public int Index { get; private set; }
        Action<EntityMainController> onCurrentState_OnStateUpdate;
        List<IAssetReferenceMasterID> loadedAssetReferences = new();
        SaveableDataEntity data;
        int layerSortingOrder;

        public void Refresh() => _ = Init(data, Index, layerSortingOrder, onCurrentState_OnStateUpdate);
        public async Task Init(SaveableDataEntity data, int index, int layerSortingOrder, Action<EntityMainController> onCurrentState_OnStateUpdate)
        {
            this.data = data;
            this.layerSortingOrder = layerSortingOrder;
            this.onCurrentState_OnStateUpdate = onCurrentState_OnStateUpdate;
            Index = index;

            Unload();
            if (data == null)
                return;
            await Load();

            teamManager.Spawn(data.GetPrefab(), init: entity => EntityInit(index, entity, data));
        }

        public virtual EntityMainController EntityInit(int index, EntityMainController entity, SaveableDataEntity saveableDataEntity)
        {
            entity.transform.SetParent(container);
            entity.Init(this, saveableDataEntity);
            entity.SetSortingOderLayer(layerSortingOrder);
            entity.CurrentState.RegisterListener(CurrentState_OnStateUpdate);
            Init(entity);
            return entity;
        }

        private void CurrentState_OnStateUpdate(EntityMainController.State state) => onCurrentState_OnStateUpdate?.Invoke(entityMainController);

        public virtual void Init(EntityMainController entityMainController)
        {
            this.entityMainController = entityMainController;
            if (!entityMainController)
                return;
            entityMainController.transform.SetParent(container);
            entityMainController.transform.localPosition = Vector3.up * 0.2f;
            entityMainController.transform.localRotation = Quaternion.identity;
            entityMainController.transform.localScale = entityMainController.Data.GetPrefab().transform.localScale;
            entityMainController.transform.DOLocalMoveY(0, 0.25f).SetDelay(Index * 0.05f).SetEase(Ease.OutBack);
        }

        public async Task Load()
        {
            loadedAssetReferences = data.GetAssetReference(assetType);
            await Task.WhenAll(loadedAssetReferences.Select(x => x.Load()));
        }

        public void Unload()
        {
            if (entityMainController)
            {
                if (teamManager)
                    teamManager.Despawn(entityMainController);
                else
                    Destroy(entityMainController.gameObject);
            }
            loadedAssetReferences?.ForEach(x => x.Unload());
            loadedAssetReferences.Clear();
        }

        protected virtual void OnDestroy() => Unload();
    }
}
