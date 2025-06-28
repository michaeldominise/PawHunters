using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EntityAnimationController : MonoBehaviour
    {
        public enum State { Idle, Walking, Running, BasicAttack, SpecialSkillAttack, SpecialSkillCast, Hurt = 8, AttackDone = 9, Dead = 10 }
         
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        [SerializeField] Animator animator;
        [SerializeField] SpriteRenderer head;
        [SerializeField] SpriteRenderer jaw;
        [SerializeField] List<Sprite> HeadSprites;
        [SerializeField] List<Sprite> JawSprites;

        EntityMainController entityMainController;

        public void Init(EntityMainController entityMainController)
        {
            this.entityMainController = entityMainController;

            var behaviours = animator.GetBehaviours<EntityAnimationControllerSetState>();
            foreach (var behaviour in behaviours)
                behaviour.Init(this);
        }

        private void OnEnable() => SetState(CurrentState.Value, force: true);

        [Button]
        public void SetState(State state, float delay = 0, bool force = false)
        {
            if (!gameObject.activeInHierarchy || (!force && CurrentState.Value == state))
                return;
            StopAllCoroutines();
            StartCoroutine(_SetState(state, delay));
        }

        IEnumerator _SetState(State state, float delay = 0)
        {
            SetHead(0);
            if(delay > 0)
                yield return new WaitForSeconds(delay);

            animator.SetInteger("State", (int)state);
            CurrentState.Value = state;
        }

        public void OnAnimationStateEnter(State state)
        {
            switch (state)
            {
                case State.Idle:
                    SetState(State.Idle);
                    break;
                case State.AttackDone:
                    SetState(State.AttackDone);
                    break;
            }
        }

        public void OnAnimationStateUpdate(State state) { }

        public void OnAnimationStateExit(State state) { }

        public void SetHead(int index)
        {
            if (head && index < HeadSprites.Count)
                head.sprite = HeadSprites[index];

            if (jaw && index < JawSprites.Count)
                jaw.sprite = JawSprites[index];
        }
    }
}
