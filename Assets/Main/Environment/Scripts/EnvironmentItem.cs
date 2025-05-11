using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class EnvironmentItem : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] layers;
        [SerializeField] Transform groundTopCenter;
        [SerializeField] Transform groundTopLeft;
        [SerializeField] Transform groundTopRight;
        [SerializeField] Transform groundBottomCenter;
        [SerializeField] int groundOrderInLayer = 5;

        public int GroundOrderInLayer => groundOrderInLayer;
        public Vector3 GroundTopCenterPosition => groundTopCenter.position;
        public Vector3 GroundTopLeftPosition => groundTopLeft.position;
        public Vector3 GroundTopRightPosition => groundTopRight.position;
        public Vector3 GroundBottomCenterPosition => groundBottomCenter.position;

        private void Start() => Init();

        [Button]
        public void Init()
        {
            groundTopLeft.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, groundTopLeft.position.y, groundTopLeft.position.z);
            groundTopRight.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right).x, groundTopLeft.position.y, groundTopLeft.position.z);
        }

    }
}
