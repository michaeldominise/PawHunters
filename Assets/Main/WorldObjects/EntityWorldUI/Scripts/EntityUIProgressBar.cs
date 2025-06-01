using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public abstract class EntityUIProgressBar : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI label;
        [SerializeField] protected Slider sliderProgress;

        protected EntityMainController entityMainController;
        protected AnimationCurve animationCurve = GlobalSettings.Instance?.gameSettings.progressUpdateAnimationCurve ?? default;
        protected float UpdateDuration => GlobalSettings.Instance.gameSettings.progressUpdateDuration;
        protected virtual Color ProgressColor => Color.white;
        Coroutine updateCoroutine;

        protected abstract float SliderCurrentValue { get; }

        public virtual void Init(EntityMainController entityMainController)
        {
            this.entityMainController = entityMainController;
            sliderProgress.value = 0;
            sliderProgress.image.color = ProgressColor;
            updateCoroutine = null;
            Refresh();
        }


        protected void Refresh()
        {
            if (updateCoroutine != null || !gameObject.activeInHierarchy)
                return;

            updateCoroutine = GradualChangeValue.Execute(sliderProgress.value, SliderCurrentValue, UpdateDuration, OnProgressUpdate, animationCurve);
        }

        protected virtual void OnProgressUpdate(GradualChangeValue.Status status)
        {
            if(label)
                SetLabel(status);
            sliderProgress.value = status.CurrentValue;
            if (status.IsDone)
                updateCoroutine = null;
        }

        protected virtual void SetLabel(GradualChangeValue.Status status) { }
    }
}
