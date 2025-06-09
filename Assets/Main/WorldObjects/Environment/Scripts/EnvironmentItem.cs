using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class EnvironmentItem : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] layers;
        [SerializeField] SpriteRenderer overlayLayer;
        [SerializeField] Transform groundTopCenter;
        [SerializeField] Transform groundMiddleCenter;
        [SerializeField] Transform groundMiddleLeft;
        [SerializeField] Transform groundMiddleRight;
        [SerializeField] Transform groundBottomCenter;
        [SerializeField] int groundOrderInLayer = 5;

        public int GroundOrderInLayer => groundOrderInLayer;
        public Vector3 GroundTopCenterPosition => groundTopCenter.position;
        public Vector3 GroundMiddleCenterPosition => groundMiddleCenter.position;
        public Vector3 GroundMiddleLeftPosition => groundMiddleLeft.position;
        public Vector3 GroundMiddleRightPosition => groundMiddleRight.position;
        public Vector3 GroundBottomCenterPosition => groundBottomCenter.position;

        [Button]
        public void Init()
        {
            groundMiddleLeft.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, groundMiddleLeft.position.y, groundMiddleLeft.position.z);
            groundMiddleRight.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right).x, groundMiddleLeft.position.y, groundMiddleLeft.position.z);
        }

        public void ShowOverlay(Color targetColor, float opacityValue, float duration) => overlayLayer.DOColor(new Color(targetColor.r, targetColor.g, targetColor.b, opacityValue), duration);
    }
}
