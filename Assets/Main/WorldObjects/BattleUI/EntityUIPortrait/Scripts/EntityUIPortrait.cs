using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityUIPortrait : MonoBehaviour
    {
        public enum State { Idle, Attacking }

        [SerializeField] EntityMainController entityMainController;
        [SerializeField] TextMeshProUGUI orderLabel;
        [SerializeField] Image avatar;
        [SerializeField] float[] scaleStates;
        [SerializeField] float duration = 0.5f;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

        GradualChangeValue.Status animStatus;
        AnimationCurve AnimationTransformCurve => GameSettings_Battle.Instance?.constantValues.bounceAnimationCurve ?? default;

        public EntityMainController EntityMainController => entityMainController;

        public void Init(EntityMainController entityMainController)
        {
            if (!entityMainController)
                return;

            if(this.entityMainController)
                this.entityMainController.CurrentState.UnregisterListener(CheckState);

            this.entityMainController = entityMainController;
            this.entityMainController.CurrentState.RegisterListener(CheckState);

            avatar.sprite = entityMainController.AvatarSprite;
            SetState(State.Idle, true);
            CheckState();
        }

        [Button]
        public void CheckState()
        {
            if (entityMainController.CurrentState.Value != EntityMainController.State.Attacking)
                SetState(State.Idle);
            else
                SetState(State.Attacking);
        }

        public void SetState(State state, bool ingnoreCurrentState = false)
        {
            if (!ingnoreCurrentState && this.CurrentState.Value == state)
                return;

            CurrentState.Value = state;

            var originalScale = transform.localScale.x;
            animStatus?.Stop();
            animStatus = GradualChangeValue.Execute(0, 1, duration,
                status => transform.localScale = Mathf.LerpUnclamped(originalScale, scaleStates[(int)CurrentState.Value], status.progress) * Vector3.one, AnimationTransformCurve);
        }

        internal void SetOrder(int i)
        {
            transform.SetSiblingIndex(i);
            orderLabel.text = $"{i + 1}";
        }
    }
}
