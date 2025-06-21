using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class JourneyButtons : SingletonMonoBehaviour<JourneyButtons>
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] ConditionalButton yesButton;
        [SerializeField] ConditionalButton noButton;

        Action<ConditionalButton.ButtonType> onClick;

        private void Start() => Hide();

        public void Init(Action<ConditionalButton.ButtonType> onClick, string buttonLabel, bool addNoButton = false)
        {
            this.onClick = onClick;
            yesButton.Init(OnClick, buttonLabel);
            noButton.Init(OnClick, "No");
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(addNoButton);
            canvasGroup.interactable = true;
        }

        void OnClick(ConditionalButton.ButtonType response)
        {
            canvasGroup.interactable = false;
            onClick?.Invoke(response);
        }

        public void Hide()
        {
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
        }
    }
}
