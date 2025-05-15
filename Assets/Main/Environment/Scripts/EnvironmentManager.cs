using System;
using UnityEngine;

namespace PawHunters
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance { get; private set; }

        [SerializeField] Vector3 characterOffset = Vector3.right;
        [SerializeField] float speed = 10f;
        [SerializeField] EnvironmentItem environmentItem;

        public int GroundOrderInLayer => environmentItem.GroundOrderInLayer;
        public Vector3 GroundTopCenterPosition => environmentItem.GroundTopCenterPosition;
        public Vector3 GroundMiddleCenterPosition => environmentItem.GroundMiddleCenterPosition;
        public Vector3 GroundMiddleLeftPosition => environmentItem.GroundMiddleLeftPosition;
        public Vector3 GroundMiddleRightPosition => environmentItem.GroundMiddleRightPosition;
        public Vector3 GroundBottomCenterPosition => environmentItem.GroundBottomCenterPosition;

        public event Action<Vector3> OnMoveCamera;

        public Vector3 InitialCharacterPosition => GroundMiddleLeftPosition + characterOffset;

        public static class GroundBoundingBox
        {
            public static float Top => Instance.GroundTopCenterPosition.y;
            public static float Left => Instance.GroundMiddleLeftPosition.x + 0.5f;
            public static float Right => Instance.GroundMiddleRightPosition.x - 0.5f;
            public static float Bottom => Instance.GroundBottomCenterPosition.y;
        }

        private void Awake() => Instance = this;
        private void Start() => Init();
        public void Init()
        {
        }

        private void Update() => MoveCamera();

        public void MoveCamera()
        {
            var movement = Vector3.right * speed * Time.deltaTime;
            Camera.main.transform.Translate(movement);
            OnMoveCamera?.Invoke(movement);
        }
    }
}
