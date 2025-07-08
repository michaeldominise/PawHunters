using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class TeamManager : Spawner<EntityMainController>
    {
        [SerializeField] protected SaveableTeamData teamData;
        [SerializeField] List<TeamManager_EntityParent> teamManger_EntityParents;
        [SerializeField] SaveableTeamData.AssetType assetType = SaveableTeamData.AssetType.All;

        protected abstract int LayerSortingOrder { get; }

        public List<EntityMainController> EntityList => teamManger_EntityParents.FindAll(x => x && x.entityMainController)?.Select(x => x.entityMainController).ToList();
        public List<EntityMainController> AliveEntityList => teamManger_EntityParents.FindAll(x => x && x.entityMainController && x.entityMainController.IsAlive)?.Select(x => x.entityMainController).ToList();
        public bool IsAlive => AliveEntityList?.FirstOrDefault(x => x.IsAlive) != null;

        List<IAssetReferenceMasterID> loadedAssetReferences;

        protected virtual void Refresh() => _ = Init(teamData);
        public virtual async Task Init(SaveableTeamData teamData)
        {
            Clear();
            this.teamData = SaveableData.Initialize(this.teamData, teamData, Refresh);
            await Load();

            for (var x = 0; x < teamData.Characters.Count && x < teamManger_EntityParents.Count; x++)
            {
                if (teamData.Characters[x] == null)
                    continue;
                Spawn(teamData.Characters[x].GetPrefab(), init: entity => EntityInit(x, entity, teamData.Characters[x]));
            }
        }

        public virtual EntityMainController EntityInit(int index, EntityMainController entity, SaveableCharacterData saveableCharacterData)
        {
            entity.Init(this, saveableCharacterData);
            entity.SetSortingOderLayer(LayerSortingOrder);
            teamManger_EntityParents[index].Init(entity);
            entity.CurrentState.RegisterListener(state => CurrentState_OnStateUpdate(entity));
            return entity;
        }

        protected virtual void CurrentState_OnStateUpdate(EntityMainController entity)
        {
            if (entity.IsAlive)
                return;

            Despawn(entity, 2);
            teamManger_EntityParents.FirstOrDefault(x => x.entityMainController == entity).Init(null);
        }

        [Button]
        public void Rearrange()
        {
            var aliveEntityList = AliveEntityList;
            if (aliveEntityList == null || aliveEntityList.Count == 0)
                return;
            aliveEntityList.Add(aliveEntityList.First());
            aliveEntityList.RemoveAt(0);
            for (var x = 0; x < teamManger_EntityParents.Count; x++)
                teamManger_EntityParents[x].Init(aliveEntityList.Count > x ? aliveEntityList[x] : null);
        }

        public override void Clear()
        {
            Unload();
            base.Clear();
        }

        public async Task Load()
        {
            loadedAssetReferences = teamData.GetAssetReference(assetType);
            await Task.WhenAll(loadedAssetReferences.Select(x => x.Load()));
        }

        public void Unload() => loadedAssetReferences?.ForEach(x => x.Unload());
        protected virtual void OnDestroy() => Unload();
    }
}
