using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance { get; private set; }

        public enum State { Idle, Walking, Running }

        [Serializable]
        public class StateSpeed
        {
            public State state;
            public float speed;
        }

        [SerializeField] EnvironmentItem environmentItem;
        [SerializeField] List<StateSpeed> stateSpeedList = new();
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        public event Action OnMove;

        public float Speed => stateSpeedList.FirstOrDefault(x => CurrentState.Value == x.state)?.speed ?? 0;
        public int GroundOrderInLayer => environmentItem.GroundOrderInLayer;
        public Vector3 GroundTopCenterPosition => environmentItem.GroundTopCenterPosition;
        public Vector3 GroundMiddleCenterPosition => environmentItem.GroundMiddleCenterPosition;
        public Vector3 GroundMiddleLeftPosition => environmentItem.GroundMiddleLeftPosition;
        public Vector3 GroundMiddleRightPosition => environmentItem.GroundMiddleRightPosition;
        public Vector3 GroundBottomCenterPosition => environmentItem.GroundBottomCenterPosition;

        public static class GroundBoundingBox
        {
            public static float Top => Instance.GroundTopCenterPosition.y;
            public static float Left => Instance.GroundMiddleLeftPosition.x + 0.5f;
            public static float Right => Instance.GroundMiddleRightPosition.x - 0.5f;
            public static float Bottom => Instance.GroundBottomCenterPosition.y;
        }

        void Awake() => Instance = this;

        [Button]
        public void SetState(State state) => CurrentState.Value = state;
        public void Init()
        {
            environmentItem.Init();
            SetState(State.Walking);
        }

        void Update() => Move();
        void Move()
        {
            var speed = Speed;
            if (speed == 0)
                return;
            var movement = speed * Time.deltaTime * Vector3.right;
            if (CurrentState.Value == State.Running && TeamManager_GameEnemy.Instance.IsAlive)
            {
                var targetPositionX = TeamManager_GameEnemy.Instance.SpawnParent.position.x + TeamManager_GamePlayer.Instance.offset.x;
                if (targetPositionX - Camera.main.transform.position.x < movement.x)
                {
                    movement = (targetPositionX - Camera.main.transform.position.x) * Vector3.right;
                    CurrentState.Value = State.Idle;
                }
            }

            Camera.main.transform.Translate(movement);
            OnMove?.Invoke();
        }
    }
}
