using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System;
using UnityEngine.Serialization;
using System.Threading.Tasks;

namespace LabHavenInteractive.PawHunters
{
    public class TeamManager_Game : TeamManager
    {
        public Vector3 offset;
        [SerializeField] EntityMainController_Team entityMainController_Team;
        [SerializeField] bool executeSkillsOnInit = true;
        [ShowInInspector, ReadOnly] public StateController<StateSpeed.State> CurrentState { get; private set; } = new();

        protected override int LayerSortingOrder => EnvironmentManager.Instance.GroundOrderInLayer;
        public EntityMainController_Team EntityMainController_Team => entityMainController_Team;
        public virtual float MovementSpeed => AppSettings_Global.Instance.constantValues.GetSpeed(CurrentState.Value);


        public event Action OnMove;

        public override void Init(SaveableTeamData teamData)
        {
            base.Init(teamData);
            EntityMainController_Team.Init(this);
            if(executeSkillsOnInit)
                _ = ExecuteSkills(GameActionTriggersManager.TriggerType.SetupPhase);
        }

        [Button]
        public virtual void SetState(StateSpeed.State state)
        {
            CurrentState.Value = state;
            activeList.ForEach(x => x.CheckState());
        }

        void Update()
        { 
            if (CurrentState.Value == StateSpeed.State.Idle)
                return;
            Move(spawnParent.transform.position + MovementSpeed * Time.deltaTime * Vector3.right);
        }

        public virtual void Move(Vector3 worldPosition)
        {
            spawnParent.transform.position = worldPosition;
            OnMove?.Invoke();
        }

        [Button]
        public void Kill() => AliveEntityList.ForEach(x => x.EntityHealthController.Kill());


        public async Task ExecuteSkills(GameActionTriggersManager.TriggerType trigger, object srouceTrigger = null, Func<bool> condition = null)
        {
            if (!IsAlive)
                return;

            await entityMainController_Team.EntitySkillsController.Execute(trigger, srouceTrigger);
            if (!(condition?.Invoke() ?? true))
                return;
            foreach (var entity in AliveEntityList)
            {
                await entity.EntitySkillsController.Execute(trigger, srouceTrigger);
                if (!(condition?.Invoke() ?? true))
                    return;
            }
        }

        public void AddSkill(SkillData skillData) => entityMainController_Team.AddSkill(skillData);
    }
}
