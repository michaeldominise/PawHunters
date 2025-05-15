using System;
using System.Collections.Generic;
using UnityEngine;

namespace PawHunters
{
    public class EntityAnimationController : StateController<EntityAnimationController.State>
    {
        public enum State { Idle, Running, Attacking, AttackDone, Dead = 10 }

        [SerializeField] Animator animator;
        [SerializeField] SpriteRenderer Head;
        [SerializeField] SpriteRenderer Jaw;
        [SerializeField] List<Sprite> HeadSprites;
        [SerializeField] List<Sprite> JawSprites;

        EntityMainController playerMainController;

        public void Init(EntityMainController playerMainController)
        {
            this.playerMainController = playerMainController;
            var behaviours = animator.GetBehaviours<EntityAnimationControllerSetState>();
            foreach (var behaviour in behaviours)
                behaviour.Init(this);
        }

        public void SetState(State state)
        {
            if (CurrentState == state)
                return;

            animator.SetInteger("State", (int)state);
            SetHead(state switch
            {
                State.Attacking => 1,
                State.Dead => 2,
                _ => 0
            });
            CurrentState = state;
        }

        public void OnAnimationStateEnter(State state)
        {
            switch (state)
            {
                case State.AttackDone:
                    SetState(State.AttackDone);
                    break;
            }
        }

        public void OnAnimationStateUpdate(State state) { }

        public void OnAnimationStateExit(State state)
        {
            switch(state)
            {
                case State.AttackDone:
                    SetState(State.Idle);
                    break;
            }
        }

        public void SetHead(int index)
        {
            if (index < HeadSprites.Count)
            {
                Head.sprite = HeadSprites[index];
            }

            if (index < JawSprites.Count)
            {
                Jaw.sprite = JawSprites[index];
            }
        }
    }
}
