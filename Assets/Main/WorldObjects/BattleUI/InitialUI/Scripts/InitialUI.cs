using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

namespace LabHaven.PawHunters
{
    public class InitialUI : SingletonMonoBehaviour<InitialUI>
    {
        [SerializeField] CanvasGroup overlay;
        [SerializeField] CanvasGroup title;

        float OverlayFadeOutDelayDuration => GameSettings_Battle.Instance.constantValues.overlayFadeOutDelayDuration;
        float OverlayFadeOutDuration => GameSettings_Battle.Instance.constantValues.overlayFadeOutDuration;
        float TitleFadeOutDelayDuration => GameSettings_Battle.Instance.constantValues.titleFadeOutDelayDuration;
        float TitleFadeOutDuration => GameSettings_Battle.Instance.constantValues.titleFadeOutDuration;

        private void Start()
        {
            overlay.alpha = 1;
            title.alpha = 1;
        }

        public async void Init()
        {
            await Task.Yield();
            overlay.DOFade(0f, OverlayFadeOutDuration).SetDelay(OverlayFadeOutDelayDuration);
            title.DOFade(0f, TitleFadeOutDuration).SetDelay(TitleFadeOutDelayDuration);
        }
    }
}
