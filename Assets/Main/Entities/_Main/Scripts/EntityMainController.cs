using System;
using System.Collections;
using Assets.FantasyMonsters.Common.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace PawHunters
{
    public class EntityMainController : MonoBehaviour
    {
        public enum State { None, Walking, Running, Attacking, Hit, Dead }

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new(State.Dead);
        [SerializeField] TeamManager teamManager;
        [SerializeField] SaveableCharacterData characterData;
        [SerializeField] Transform model;
        [SerializeField] Transform worldUIPoint;
        [SerializeField] LayerManager layerManager;
        [SerializeField] EntitySkillsController entitySkillsController;
        [SerializeField] EntityHealthController entityHealthController;
        [SerializeField] EntityMovementController entityMovementController;
        [SerializeField] EntityAnimationController entityAnimationController;

        public SaveableCharacterData CharacterData => characterData;
        public Transform Model => model;
        public Transform WorldUIPoint => worldUIPoint;
        public EntitySkillsController EntitySkillsController => entitySkillsController;
        public EntityHealthController EntityHealthController => entityHealthController;
        public EntityMovementController EntityMovementController => entityMovementController;
        public EntityAnimationController EntityAnimationController => entityAnimationController;
        TeamManager_GamePlayer TeamManager_GamePlayer => teamManager as TeamManager_GamePlayer;

        void Refresh() => Init(teamManager, characterData);
        public void Init(TeamManager teamManager, SaveableCharacterData characterData)
        {
            this.teamManager = teamManager;
            this.characterData = characterData;
            characterData.RegisterOnValueChange(Refresh);

            RegisterListener();

            gameObject.SetActive(true);
            //gameObject.name = $"{gameObject.name.TrimEnd(':')}:{characterData.name}";

            entityMovementController.Init(this);
            entitySkillsController.Init(this);
            entityHealthController.Init(this);
            entityAnimationController.Init(this);

            layerManager.SetSortingGroupOrder(EnvironmentManager.Instance.GroundOrderInLayer);

            SetToIdle();
        }

        private void RegisterListener()
        {
            entityMovementController.CurrentState.OnStateUpdateClear();
            entitySkillsController.CurrentState.OnStateUpdateClear();
            entityHealthController.CurrentState.OnStateUpdateClear();
            entityHealthController.CurrentState.OnStateUpdateClear();

            entityMovementController.CurrentState.OnStateUpdate += state => CheckState();
            entitySkillsController.CurrentState.OnStateUpdate += state => CheckState();
            entityHealthController.CurrentState.OnStateUpdate += state => CheckState();
            entityHealthController.CurrentState.OnStateUpdate += state => CheckState();

            if (TeamManager_GamePlayer)
            {
                EnvironmentManager.Instance.CurrentState.OnStateUpdate -= TeamManager_Game_OnStateUpdate;
                EnvironmentManager.Instance.CurrentState.OnStateUpdate += TeamManager_Game_OnStateUpdate;
            }
        }

        void TeamManager_Game_OnStateUpdate(EnvironmentManager.State teamState) => CheckState();
        public void CheckState()
        {
            if (CurrentState.Value == State.Dead)
                return;

            if (EntityHealthController.CurrentState.Value == EntityHealthController.State.Dead)
                SetToDead();
            else if (EntitySkillsController.CurrentState.Value == EntitySkillsController.State.Attacking)
                SetToAttacking();
            else if (TeamManager_GamePlayer && EnvironmentManager.Instance.CurrentState.Value != EnvironmentManager.State.Idle)
                SetToMoving();
            else
                SetToIdle();
        }

        void SetToIdle()
        {
            CurrentState.Value = State.None;
            EntityAnimationController.SetState(EntityAnimationController.State.Idle);
        }

        void SetToMoving()
        {
            switch (EnvironmentManager.Instance.CurrentState.Value)
            {
                case EnvironmentManager.State.Walking:
                    CurrentState.Value = State.Walking;
                    EntityAnimationController.SetState(EntityAnimationController.State.Walking,  UnityEngine.Random.Range(0, 0.25f));
                    break;
                case EnvironmentManager.State.Running:
                    CurrentState.Value = State.Running;
                    EntityAnimationController.SetState(EntityAnimationController.State.Running,  UnityEngine.Random.Range(0, 0.25f));
                    break;
                default:
                    break;
            }
            
        }

        void SetToAttacking()
        {
            CurrentState.Value = State.Attacking;
            EntityAnimationController.SetState(EntityAnimationController.State.Attacking);
        }

        void SetToDead() => StartCoroutine(_SetToDead());
        IEnumerator _SetToDead()
        {
            CurrentState.Value = State.Dead;
            EntityAnimationController.SetState(EntityAnimationController.State.Dead);
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
        }
    }
}
