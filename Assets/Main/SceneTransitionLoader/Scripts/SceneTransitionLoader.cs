using UnityEngine;
using DG.Tweening;

namespace LabHavenInteractive.PawHunters
{
    public class SceneTransitionLoader : SingletonMonoBehaviour<SceneTransitionLoader>
    {
        [SerializeField] GameObject container;
        [SerializeField] CanvasGroup canvasGroup;

        float FadeInDuration => AppSettings_Global.Instance.uIAnimationValues.effectDuration;

        public void Show(float delay = 0) => SetValue(true, delay);
        public void Hide(float delay = 0) => SetValue(false, delay);
        void SetValue(bool isShow, float delay)
        {
            if(isShow)
                container.SetActive(true);
            canvasGroup.blocksRaycasts = isShow;
            canvasGroup.interactable = isShow;
            canvasGroup.DOFade(isShow ? 1 : 0, FadeInDuration).OnComplete(() => container.SetActive(isShow)).SetDelay(delay);
        }
    }
}
