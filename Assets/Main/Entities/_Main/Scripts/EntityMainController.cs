using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace LabHaven.PawHunters
{
    public class EntityMainController : MonoBehaviour
    {
        public enum State { None, Walking, Running, Attacking, Dead }

        [Serializable]
        public class AnchorGroup
        {
            public Transform worldUI;
            public Transform statusTextUI;
            public Transform body;
            public Transform weapon;
        }

        [SerializeField] protected SaveableCharacterData characterData;
        [SerializeField] protected ElementType element;
        [SerializeField] protected Sprite avatarSprite;
        [SerializeField] protected TeamManager teamManager;
        [SerializeField] protected Transform model;
        [SerializeField] protected SortingGroup sortingGroup;
        [SerializeField] protected EntitySkillsController entitySkillsController;
        [SerializeField] protected EntityHealthController entityHealthController;
        [SerializeField] protected EntityMovementController entityMovementController;
        [SerializeField] protected EntityAnimationController entityAnimationController;
        [SerializeField] protected EntityStatusEffectController entityStatusEffectController;
        [SerializeField] protected AnchorGroup anchor;
        [SerializeField] protected BattleAttributes battleAttributes;

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new(State.None);

        public SaveableCharacterData CharacterData => characterData;
        public ElementType Element => element;
        public Sprite AvatarSprite => avatarSprite;
        public Transform Model => model;
        public EntitySkillsController EntitySkillsController => entitySkillsController;
        public EntityHealthController EntityHealthController => entityHealthController;
        public EntityMovementController EntityMovementController => entityMovementController;
        public EntityAnimationController EntityAnimationController => entityAnimationController;
        public EntityStatusEffectController EntityStatusEffectController => entityStatusEffectController;
        public TeamManager_Game TeamManager_Game => teamManager as TeamManager_Game;
        public TeamManager_GamePlayer TeamManager_GamePlayer => teamManager as TeamManager_GamePlayer;
        public TeamManager_GameEnemy TeamManager_GameEnemy => teamManager as TeamManager_GameEnemy;
        public AnchorGroup Anchor => anchor;

        public virtual BattleAttributes BattleAttributes => battleAttributes;
        public virtual bool IsAlive => CurrentState.Value != State.Dead;

        void Refresh() => Init(teamManager, characterData);
        public virtual void Init(TeamManager teamManager, SaveableCharacterData characterData)
        {
            this.teamManager = teamManager;
            gameObject.name = $"{gameObject.name.Split(':')[0]}:{(TeamManager_GamePlayer ? "Player" : "Enemy")}";

            SaveableData.Initialize(ref this.characterData, characterData, Refresh);
            battleAttributes.Init(characterData.attribute);
            RegisterListener();

            entityHealthController.Init(this);
            entityMovementController.Init(this);
            entityAnimationController.Init(this);
            EntityStatusEffectController.Init(this);
            entitySkillsController.Init(this);

            if (CurrentState.Value == State.Dead)
                SetToIdle();
            CheckState();
        }

        public void SetSortingOderLayer(int sortingOrder) => sortingGroup.sortingOrder = sortingOrder;

        private void RegisterListener()
        {
            CurrentState.ClearListeners();
            entitySkillsController.CurrentState.ClearListeners();
            entityHealthController.CurrentState.ClearListeners();

            entitySkillsController.CurrentState.RegisterListener(CheckState);
            entityHealthController.CurrentState.RegisterListener(CheckState);

            if (TeamManager_Game)
                TeamManager_Game.CurrentState.RegisterListener(CheckState);
        }

        public void CheckState()
        {
            if (CurrentState.Value == State.Dead)
                return;

            if (EntityHealthController.CurrentState.Value == EntityHealthController.State.Dead)
                SetToDead();
            else if (EntitySkillsController.CurrentState.Value == EntitySkillsController.State.Attacking)
                SetToAttacking();
            else if (TeamManager_Game && TeamManager_Game.CurrentState.Value != StateSpeed.State.Idle)
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
            switch (TeamManager_Game.CurrentState.Value)
            {
                case StateSpeed.State.Walking:
                    CurrentState.Value = State.Walking;
                    EntityAnimationController.SetState(EntityAnimationController.State.Walking, UnityEngine.Random.Range(0, 0.25f));
                    break;
                case StateSpeed.State.Running:
                    CurrentState.Value = State.Running;
                    EntityAnimationController.SetState(EntityAnimationController.State.Running, UnityEngine.Random.Range(0, 0.25f));
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
