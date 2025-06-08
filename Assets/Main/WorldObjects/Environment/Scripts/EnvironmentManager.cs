using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class EnvironmentManager : SingletonMonoBehaviour<EnvironmentManager>
    {
        [SerializeField] Transform cameraTransform;
        [SerializeField] EnvironmentItem environmentItem;

        public EnvironmentItem EnvironmentItem => environmentItem;
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

        public void Init(EnvironmentItem environmentItem)
        {
            this.environmentItem = Instantiate(environmentItem, cameraTransform);
            this.environmentItem.transform.localPosition = Vector3.forward * 10;
            this.environmentItem.Init();
        }

        public void Move(Vector3 worldPosition) => cameraTransform.position = worldPosition;
    }
}
