using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

namespace LabHavenInteractive.PawHunters
{
    public class SceneTransitionLoader : SingletonMonoBehaviour<SceneTransitionLoader>
    {
        [SerializeField] GameObject container;
        [SerializeField] CanvasGroup canvasGroup;

        float FadeInDuration => AppSettings_Global.Instance.uIAnimationValues.effectDuration;

        public async Task Show(float delay = 0) => await SetValue(true, delay);
        public async Task Hide(float delay = 0) => await SetValue(false, delay);
        async Task SetValue(bool isShow, float delay)
        {
            if(isShow)
                container.SetActive(true);
            canvasGroup.blocksRaycasts = isShow;
            canvasGroup.interactable = isShow;
            canvasGroup.DOFade(isShow ? 1 : 0, FadeInDuration).OnComplete(() => container.SetActive(isShow)).SetDelay(delay);
            await Task.Delay((int)((FadeInDuration + delay) * 1000));
        }
    }
}
