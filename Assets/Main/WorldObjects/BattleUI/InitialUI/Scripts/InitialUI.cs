using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;

namespace LabHavenInteractive.PawHunters
{
    public class InitialUI : SingletonMonoBehaviour<InitialUI>
    {
        [SerializeField] CanvasGroup overlay;
        [SerializeField] CanvasGroup title;
        [SerializeField] TextMeshProUGUI titleLabel;

        float OverlayFadeOutDelayDuration => AppSettings_Battle.Instance.constantValues.overlayFadeOutDelayDuration;
        float OverlayFadeOutDuration => AppSettings_Battle.Instance.constantValues.overlayFadeOutDuration;
        float TitleFadeOutDelayDuration => AppSettings_Battle.Instance.constantValues.titleFadeOutDelayDuration;
        float TitleFadeOutDuration => AppSettings_Battle.Instance.constantValues.titleFadeOutDuration;

        private void Start()
        {
            overlay.alpha = 1;
            title.alpha = 1;
        }

        public async void Init(string titleText)
        {
            titleLabel.text = titleText;
            await Task.Yield();

            if (!AppSettings_Battle.Instance)
                return;
            overlay.DOFade(0f, OverlayFadeOutDuration).SetDelay(OverlayFadeOutDelayDuration);
            title.DOFade(0f, TitleFadeOutDuration).SetDelay(TitleFadeOutDelayDuration);
        }
    }
}
