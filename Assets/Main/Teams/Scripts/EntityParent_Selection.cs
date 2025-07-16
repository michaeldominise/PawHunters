using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class EntityParent_Selection : EntityParent
    {
        public enum State { NotSelected, Selected, Locked, Unlocked, RemoveClicked }

        [SerializeField] List<Graphic> selecteableGraphics;
        [SerializeField] Button selectButton;
        [SerializeField] Button removeButton;
        [SerializeField] AnimationUI selectedAnimationUI;

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

        AppSettings_Global.ColorTheme ColorTheme => AppSettings_Global.Instance.colorTheme;

        public void Start()
        {
            selectButton.onClick.AddListener(OnPlatformClick);
            removeButton.onClick.AddListener(OnRemoveClick);
        }

        private void OnEnable() => SetState(State.NotSelected);

        public void SetValue(Action<EntityParent_Selection> onStateChange, bool isLocked)
        {
            CurrentState.ClearListeners();
            SetState(isLocked ? State.Locked : State.NotSelected);
            CurrentState.RegisterListener(state => onStateChange?.Invoke(this));
        }

        public void SetState(State state)
        {
            if (CurrentState.Value == State.Locked && state != State.Unlocked)
                return;

            removeButton.gameObject.SetActive(state == State.Selected);
            selecteableGraphics.ForEach(x => x.color = state == State.Selected ? ColorTheme.tabSelectedText : ColorTheme.tabNotSelectedText_PlayerTeam);
            if (state == State.Selected)
                selectedAnimationUI.CanvasFadeIn();
            else
                selectedAnimationUI.CanvasFadeOut();

            CurrentState.Value = state;
            if (state == State.RemoveClicked || state == State.Unlocked)
                SetState(State.NotSelected);
        }

        private void OnRemoveClick() => SetState(State.RemoveClicked);

        private void OnPlatformClick()
        {
            if (CurrentState.Value == State.Locked)
                return;
            SetState(CurrentState.Value != State.Selected ? State.Selected : State.NotSelected);
        }
    }
}
