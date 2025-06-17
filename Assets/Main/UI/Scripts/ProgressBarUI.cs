using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public abstract class ProgressBarUI : MonoBehaviour
    {
        public enum OverflowType { Clamp, Repeat, PingPong } 

        [SerializeField] protected TextMeshProUGUI label;
        [SerializeField] protected Slider sliderProgress;
        [SerializeField] OverflowType sliderOverflow;

        protected virtual AnimationCurve AnimationCurve => AppSettings_Battle.Instance.constantValues.progressUpdateAnimationCurve;
        protected virtual float UpdateDuration => AppSettings_Battle.Instance.constantValues.progressUpdateDuration;
        protected virtual Color ProgressColor => Color.white;
        GradualChangeValue.Status updateStatus;

        protected abstract float CurrentValue { get; }
        protected abstract float MaxValue { get; }

        public virtual void Init()
        {
            sliderProgress.value = 0;
            sliderProgress.image.color = ProgressColor;
            updateStatus = null;
            Refresh();
        }

        protected void Refresh()
        {
            if (!gameObject.activeInHierarchy)
                return;

            sliderProgress.maxValue = MaxValue;

            updateStatus?.Stop();
            updateStatus = GradualChangeValue.Execute(sliderProgress.value, CurrentValue, UpdateDuration, OnProgressUpdate, AnimationCurve);
        }

        protected virtual void OnProgressUpdate(GradualChangeValue.Status status)
        {
            if(label)
                SetLabel(status);
            UpdateSlider(status);
            if (status.IsDone)
                updateStatus = null;
        }

        protected virtual void SetLabel(GradualChangeValue.Status status) => label.text = GetOverflowValue(status.CurrentValue).ToString("0");
        protected virtual void UpdateSlider(GradualChangeValue.Status status) => sliderProgress.value = GetOverflowValue(status.CurrentValue);
        protected float GetOverflowValue(float value)
            => sliderOverflow switch
            {
                OverflowType.Repeat => Mathf.Repeat(value, MaxValue),
                OverflowType.PingPong => Mathf.PingPong(value, MaxValue),
                _ => Mathf.Min(value, MaxValue)
            };
    }
}
