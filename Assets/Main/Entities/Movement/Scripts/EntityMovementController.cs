using System;
using System.Threading.Tasks;
using Assets.FantasyMonsters.Common.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PawHunters
{
    public abstract class EntityMovementController : MonoBehaviour
    {
        public enum State { Idle, Moving }

        [SerializeField] float moveDuration = 1f;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        protected EntityMainController entityMainController;

        public virtual void Init(EntityMainController playerMainController) => this.entityMainController = playerMainController;

        public async Task MoveToStartPosition(bool isRunning = true) => await MoveTo(Vector3.zero, isRunning, true);
        public async Task MoveToFront(EntityMainController moveTo, bool isRunning = true) => await MoveTo(moveTo.transform.TransformPoint(Vector3.right * 4), isRunning, false);
        public async Task MoveTo(Vector3 moveTo, bool isRunning = true, bool isLocal = false)
        {
            if (CurrentState.Value == State.Moving || !entityMainController.IsAlive)
                return;
            CurrentState.Value = State.Moving;
            entityMainController.EntityAnimationController.SetState(isRunning ? EntityAnimationController.State.Running : EntityAnimationController.State.Walking);
            var isFinished = false;

            if(isLocal)
                entityMainController.transform.DOLocalMove(moveTo, moveDuration).OnStepComplete(() => isFinished = true);
            else
                entityMainController.transform.DOMove(moveTo, moveDuration).OnStepComplete(() => isFinished = true);

            while (!isFinished)
                await Task.Yield();
            entityMainController.EntityAnimationController.SetState(EntityAnimationController.State.Idle);
            CurrentState.Value = State.Idle;
        }

    }
}
