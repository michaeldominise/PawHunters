using UnityEngine;

namespace PawHunters
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance { get; private set; }

        [SerializeField] Vector3 characterOffset = Vector3.right;
        [SerializeField] float speed = 10f;
        public EnvironmentItem environmentItem;

        public int GroundOrderInLayer => environmentItem.GroundOrderInLayer;
        public Vector3 GroundTopCenterPosition => environmentItem.GroundTopCenterPosition;
        public Vector3 GroundTopLeftPosition => environmentItem.GroundTopLeftPosition;
        public Vector3 GroundTopRightPosition => environmentItem.GroundTopRightPosition;
        public Vector3 GroundBottomCenterPosition => environmentItem.GroundBottomCenterPosition;

        public Vector3 GetInitialCharacterPosition() => GroundTopLeftPosition + characterOffset;

        public static class GroundBoundingBox
        {
            public static float Top => Instance.GroundTopCenterPosition.y;
            public static float Left => Instance.GroundTopLeftPosition.x + 0.5f;
            public static float Right => Instance.GroundTopRightPosition.x - 0.5f;
            public static float Bottom => Instance.GroundBottomCenterPosition.y;
        }

        private void Awake() => Instance = this;
        private void Start() => Init();
        public void Init()
        {
        }

        public void MoveCameraForward()
        {
            Camera.main.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
