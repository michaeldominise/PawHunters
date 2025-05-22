using System;
using UnityEngine;

namespace PawHunters
{
    public class JourneyButtons : MonoBehaviour
    {
        public static JourneyButtons Instance { get; private set; }

        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] ConditionalButton yesButton;
        [SerializeField] ConditionalButton noButton;

        Action<ConditionalButton.ButtonType> onClick;

        private void Awake() => Instance = this;

        public void Init(Action<ConditionalButton.ButtonType> onClick, string buttonLabel, bool addNoButton = false)
        {
            this.onClick = onClick;
            yesButton.Init(OnClick, buttonLabel);
            noButton.Init(OnClick, "No");
            noButton.gameObject.SetActive(addNoButton);
            canvasGroup.interactable = true;
        }

        void OnClick(ConditionalButton.ButtonType response)
        {
            onClick?.Invoke(response);
            canvasGroup.interactable = false;
        }
    }
}
