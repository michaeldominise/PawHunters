using System;
using System.Collections;
using Assets.FantasyMonsters.Common.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace PawHunters
{
    public class EntityMainController : StateController<EntityMainController.State>
    {
        public enum State { None, Moving, Attacking, Dead }

        [SerializeField] CharacterData characterData;
        [SerializeField] Transform model;
        [SerializeField] Transform worldUIPoint;
        [SerializeField] LayerManager layerManager;
        [SerializeField] EntitySkillsController entitySkillsController;
        [SerializeField] EntityHealthController entityHealthController;
        [SerializeField] EntityMovementController entityMovementController;
        [SerializeField] EntityAnimationController entityAnimationController;

        [ShowInInspector, ReadOnly] public bool IsRegisterListenerDone { get; private set; }

        public CharacterData CharacterData => characterData;
        public Transform Model => model;
        public Transform WorldUIPoint => worldUIPoint;
        public EntitySkillsController EntitySkillsController => entitySkillsController;
        public EntityHealthController EntityHealthController => entityHealthController;
        public EntityMovementController EntityMovementController => entityMovementController;
        public EntityAnimationController EntityAnimationController => entityAnimationController;

        [Button]
        public void Init(CharacterData characterData)
        {
            if (!IsRegisterListenerDone)
                RegisterListener();

            this.characterData = characterData;

            gameObject.SetActive(true);
            gameObject.name = $"{gameObject.name.TrimEnd(':')}:{characterData.name}";

            entityMovementController.Init(this);
            entitySkillsController.Init(this);
            entityHealthController.Init(this);
            entityAnimationController.Init(this);

            layerManager.SetSortingGroupOrder(EnvironmentManager.Instance.GroundOrderInLayer);
            transform.position = EnvironmentManager.Instance.InitialCharacterPosition;

            SetToIdle();
        }

        private void RegisterListener()
        {
            IsRegisterListenerDone = true;
            entityMovementController.OnStateUpdate += state => CheckState();
            entitySkillsController.OnStateUpdate += state => CheckState();
            entityHealthController.OnStateUpdate += state => CheckState();
        }

        public void CheckState()
        {
            if (CurrentState == State.Dead)
                return;

            if (EntityHealthController.CurrentState == EntityHealthController.State.Dead)
                SetToDead();
            else if (EntitySkillsController.CurrentState == EntitySkillsController.State.Attacking)
                SetToAttacking();
            else if (EntityMovementController.CurrentState == EntityMovementController.State.Moving)
                SetToMoving();
            else
                SetToIdle();
        }

        void SetToIdle()
        {
            CurrentState = State.None;
            EntityAnimationController.SetState(EntityAnimationController.State.Idle);
        }

        void SetToMoving()
        {
            CurrentState = State.Moving;
            EntityAnimationController.SetState(EntityAnimationController.State.Running);
        }

        void SetToAttacking()
        {
            CurrentState = State.Attacking;
            EntityAnimationController.SetState(EntityAnimationController.State.Attacking);
        }

        void SetToDead() => StartCoroutine(_SetToDead());
        IEnumerator _SetToDead()
        {
            CurrentState = State.Dead;
            EntityAnimationController.SetState(EntityAnimationController.State.Dead);
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
        }
    }
}
