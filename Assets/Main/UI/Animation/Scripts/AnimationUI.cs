using DG.Tweening;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class AnimationUI : MonoBehaviour
    {
        [SerializeField] protected Component target;
        [SerializeField] protected Ease ease = Ease.OutQuad;

        protected virtual AnimationCurve AnimationCurve => AppSettings_Global.Instance.uIAnimationValues.genericAnimationCurve;
        protected float Duration => AppSettings_Global.Instance.uIAnimationValues.effectDuration;

        public T GetComponent<T>(Component target) where T : Component => target as T;
        public void ScaleNormal() => GetComponent<Transform>(target)?.DOScale(AppSettings_Global.Instance.uIAnimationValues.scaleNormal, Duration).SetEase(ease);
        public void ScaleUp() => GetComponent<Transform>(target)?.DOScale(AppSettings_Global.Instance.uIAnimationValues.scaleUp, Duration).SetEase(ease);
        public void ScaleDown() => GetComponent<Transform>(target)?.DOScale(AppSettings_Global.Instance.uIAnimationValues.scaleDown, Duration).SetEase(ease);
        public void CanvasFadeOut() => GetComponent<CanvasGroup>(target)?.DOFade(0, Duration).SetEase(ease);
        public void CanvasFadeIn() => GetComponent<CanvasGroup>(target)?.DOFade(1, Duration).SetEase(ease);
    }
}
