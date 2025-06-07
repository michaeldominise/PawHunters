using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

namespace PawHunters
{
    public class InitialUI : MonoBehaviour
    {
        public static InitialUI Instance { get; private set; }

        [SerializeField] CanvasGroup overlay;
        [SerializeField] CanvasGroup title;

        float OverlayFadeOutDelayDuration => GameSettings_Battle.Instance.constantValues.overlayFadeOutDelayDuration;
        float OverlayFadeOutDuration => GameSettings_Battle.Instance.constantValues.overlayFadeOutDuration;
        float TitleFadeOutDelayDuration => GameSettings_Battle.Instance.constantValues.titleFadeOutDelayDuration;
        float TitleFadeOutDuration => GameSettings_Battle.Instance.constantValues.titleFadeOutDuration;

        void Awake() => Instance = this;
        private void Start()
        {
            overlay.alpha = 1;
            title.alpha = 1;
        }

        public async void Init()
        {
            await Task.Yield();
            DOTween.To(() => 1f, value => overlay.alpha = value, 0f, OverlayFadeOutDuration).SetDelay(OverlayFadeOutDelayDuration);
            DOTween.To(() => 1f, value => title.alpha = value, 0f, TitleFadeOutDuration).SetDelay(TitleFadeOutDelayDuration);
        }
    }
}
