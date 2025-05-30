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
        public enum State { None, Walking, Running, Attacking, Dead }

        [SerializeField] SaveableCharacterData characterData;
        [SerializeField] ElementType element;
        [SerializeField] Sprite avatarSprite;
        [SerializeField] TeamManager teamManager;
        [SerializeField] Transform model;
        [SerializeField] Transform worldUIPoint;
        [SerializeField] LayerManager layerManager;
        [SerializeField] EntitySkillsController entitySkillsController;
        [SerializeField] EntityHealthController entityHealthController;
        [SerializeField] EntityMovementController entityMovementController;
        [SerializeField] EntityAnimationController entityAnimationController;
        [SerializeField] EntityStatusEffectController entityStatusEffectController;
        [SerializeField] BattleAttributes battleAttributes;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new(State.None);

        public SaveableCharacterData CharacterData => characterData;
        public ElementType Element => element;
        public Sprite AvatarSprite => avatarSprite;
        public BattleAttributes BattleAttributes => battleAttributes;
        public Transform Model => model;
        public Transform WorldUIPoint => worldUIPoint;
        public EntitySkillsController EntitySkillsController => entitySkillsController;
        public EntityHealthController EntityHealthController => entityHealthController;
        public EntityMovementController EntityMovementController => entityMovementController;
        public EntityAnimationController EntityAnimationController => entityAnimationController;
        public EntityStatusEffectController EntityStatusEffectController => entityStatusEffectController;
        public TeamManager_GamePlayer TeamManager_GamePlayer => teamManager as TeamManager_GamePlayer;
        public bool IsAlive => CurrentState.Value != State.Dead;


        void Refresh() => Init(teamManager, characterData);
        public void Init(TeamManager teamManager, SaveableCharacterData characterData)
        {
            this.teamManager = teamManager;
            SaveableData.Initialize(ref this.characterData, characterData, Refresh);
            battleAttributes.Init(characterData.attribute);
            RegisterListener();

            gameObject.SetActive(true);
            gameObject.name = $"{gameObject.name.TrimEnd(':')}:{(TeamManager_GamePlayer ? "Player" : "Enemy")}";

            entitySkillsController.Init(this);
            entityHealthController.Init(this);
            entityMovementController.Init(this);
            entityAnimationController.Init(this);
            EntityStatusEffectController.Init(this);

            layerManager.SetSortingGroupOrder(EnvironmentManager.Instance.GroundOrderInLayer);

            CheckState();
        }

        private void RegisterListener()
        {
            CurrentState.ClearListeners();
            entitySkillsController.CurrentState.ClearListeners();
            entityHealthController.CurrentState.ClearListeners();

            entitySkillsController.CurrentState.RegisterListener(CheckState);
            entityHealthController.CurrentState.RegisterListener(CheckState);

            if (TeamManager_GamePlayer)
                EnvironmentManager.Instance.CurrentState.RegisterListener(CheckState);
        }

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

        void SetToAttacking() => CurrentState.Value = State.Attacking;

        void SetToDead()
        {
            CurrentState.Value = State.Dead;
            EntityAnimationController.SetState(EntityAnimationController.State.Dead);
            EntityStatusEffectController.Clear();
        }
    }
}
