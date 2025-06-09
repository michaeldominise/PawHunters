using System;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class JourneyButtons : SingletonMonoBehaviour<JourneyButtons>
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] ConditionalButton yesButton;
        [SerializeField] ConditionalButton noButton;

        Action<ConditionalButton.ButtonType> onClick;

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
