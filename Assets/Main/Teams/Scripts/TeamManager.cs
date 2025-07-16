using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabHavenInteractive.PawHunters
{
    public abstract class TeamManager : Spawner<EntityMainController>
    {
        [SerializeField] protected SaveableTeamData teamData;
        [SerializeField, FormerlySerializedAs("teamManger_EntityParents")] protected List<EntityParent> entityParents;

        protected abstract int LayerSortingOrder { get; }

        public List<EntityMainController> EntityList => entityParents.FindAll(x => x && x.entityMainController)?.Select(x => x.entityMainController).ToList();
        public List<EntityMainController> AliveEntityList => entityParents.FindAll(x => x && x.entityMainController && x.entityMainController.IsAlive)?.Select(x => x.entityMainController).ToList();
        public bool IsAlive => AliveEntityList?.FirstOrDefault(x => x.IsAlive) != null;

        protected virtual void Refresh() => _ = Init(teamData);
        public virtual async Task Init(SaveableTeamData teamData)
        {
            Clear();

            this.teamData = teamData;
            var tasks = new List<Task>();
            for (var x = 0; x < teamData.Characters.Count && x < entityParents.Count; x++)
                tasks.Add(EntityLoad(x));

            await Task.WhenAll(tasks);
        }

        protected virtual void CurrentState_OnStateUpdate(EntityMainController entity)
        {
            if (entity.IsAlive)
                return;

            Despawn(entity, 2);
            entityParents.FirstOrDefault(x => x.entityMainController == entity).Init(null);
        }

        [Button]
        public void Rearrange()
        {
            var aliveEntityList = AliveEntityList;
            if (aliveEntityList == null || aliveEntityList.Count == 0)
                return;
            aliveEntityList.Add(aliveEntityList.First());
            aliveEntityList.RemoveAt(0);
            for (var x = 0; x < entityParents.Count; x++)
                entityParents[x].Init(aliveEntityList.Count > x ? aliveEntityList[x] : null);
        }

        public virtual async Task EntityLoad(int index) => await entityParents[index].Init(teamData.Characters[index], index, LayerSortingOrder, CurrentState_OnStateUpdate);
    }
}
