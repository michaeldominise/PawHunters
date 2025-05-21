using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityWorldUI : MonoBehaviour
    {
        public enum State { Alive, Dead }

        [SerializeField] EntityMainController entityMainController;
        [SerializeField] EntityHealthBar entityHealthBar;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

        public EntityMainController EntityMainController => entityMainController;
        Camera WorldCamera => Camera.main;
        RectTransform RectParent => transform.parent as RectTransform;

        public void Init(EntityMainController entityMainController)
        {
            this.entityMainController = entityMainController;
            CurrentState.Value = State.Alive;
            gameObject.SetActive(true);
            entityHealthBar.Init();
        }

        private void LateUpdate()
        {
            var viewportPoint = WorldCamera.WorldToViewportPoint(entityMainController.WorldUIPoint.position);
            var halfScreenSize = new Vector3(RectParent.rect.width, RectParent.rect.height) * 0.5f;
            transform.localPosition = new Vector3(Mathf.LerpUnclamped(-halfScreenSize.x, halfScreenSize.x, viewportPoint.x), Mathf.LerpUnclamped(-halfScreenSize.y, halfScreenSize.y, viewportPoint.y));
        }

        public void Kill()
        {
            if (CurrentState.Value == State.Dead)
                return;
            StartCoroutine(_Kill());
        }

        IEnumerator _Kill()
        {
            CurrentState.Value = State.Dead;
            yield return new WaitForSeconds(entityHealthBar.UpdateDuration);
            gameObject.SetActive(false);
        }
    }
}
