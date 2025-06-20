using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace LabHaven.PawHunters
{
    [RequireComponent(typeof(Button))]
    public class MenuItem : MonoBehaviour
    {
        public enum State { None, Selected, NotSelected }

        [ShowInInspector, HideReferenceObjectPicker, ReadOnly] StateController<State> CurrentState { get; set; } = new(State.None);

        [SerializeField] Button button;
        [SerializeField] UnityEvent onSelect;
        [SerializeField] UnityEvent onDeselect;

        Action<MenuItem> onMenuSelect;

        private void Reset() => button = GetComponent<Button>();
        private void Start() => button.onClick.AddListener(() => onMenuSelect?.Invoke(this));
        public void Init(Action<MenuItem> onSelect, bool isSelected)
        {
            onMenuSelect = onSelect;
            SetState(isSelected ? State.Selected : State.NotSelected);
        }

        public void SetState(State state)
        {
            if (CurrentState.Value == state)
                return;

            switch (state)
            {
                case State.Selected:
                    OnSelect();
                    break;
                case State.NotSelected:
                    OnDeselect();
                    break;
            }
            CurrentState.Value = state;
        }

        protected virtual void OnSelect() => onSelect?.Invoke();
        protected virtual void OnDeselect() => onDeselect?.Invoke();
    }
}
