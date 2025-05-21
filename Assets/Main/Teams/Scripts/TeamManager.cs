using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class TeamManager : Spawner<EntityMainController>
    {
        [SerializeField] SaveableTeamData teamData;
        [SerializeField] EntityMainController prefab;
        [SerializeField] List<TeamManger_EntityParent> teamManger_EntityParents;

        public bool IsAlive => spawnedList.FirstOrDefault(x => x.CurrentState.Value != EntityMainController.State.Dead) != null;
        public List<EntityMainController> AliveEntityList => teamManger_EntityParents.FindAll(x => x && x.entityMainController && x.entityMainController.CurrentState.Value != EntityMainController.State.Dead)?.Select(x => x.entityMainController).ToList();


        void Refresh() => Init(teamData);
        public void Init(SaveableTeamData teamData)
        {
            Clear();
            this.teamData?.UnregisterOnValueChange(Refresh);
            this.teamData = teamData;
            teamData.RegisterOnValueChange(Refresh);

            for (var x = 0; x < teamData.characters.Count; x++)
                Spawn(prefab, init: entity => EntityInit(x, entity, teamData.characters[x]));
        }

        public virtual EntityMainController EntityInit(int index, EntityMainController entity, SaveableCharacterData saveableCharacterData)
        {
            entity.Init(this, saveableCharacterData);
            teamManger_EntityParents[index].Init(entity);
            entity.EntityHealthController.CurrentState.OnStateUpdate += state => CurrentState_OnStateUpdate(entity);
            return entity;
        }

        protected virtual void CurrentState_OnStateUpdate(EntityMainController entity)
        {
            if (entity.CurrentState.Value != EntityMainController.State.Dead)
                return;

            entity.transform.parent = transform;
            teamManger_EntityParents.FirstOrDefault(x => x.entityMainController == entity).Init(null);
        }

        [Button]
        public void Rearrange()
        {
            var aliveEntityList = AliveEntityList;
            if (aliveEntityList == null || aliveEntityList.Count == 0)
                return;
            aliveEntityList.Insert(0, aliveEntityList.Last());
            aliveEntityList.RemoveAt(aliveEntityList.Count - 1);
            for (var x = 0; x < teamManger_EntityParents.Count; x++)
                teamManger_EntityParents[x].Init(aliveEntityList.Count > x ? aliveEntityList[x] : null);
        }

        public override void Clear()
        {
            spawnedList.ForEach(x => x.transform.parent = transform);
            base.Clear();
        }
    }
}
