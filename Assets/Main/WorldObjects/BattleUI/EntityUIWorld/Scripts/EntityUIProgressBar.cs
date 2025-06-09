using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public abstract class EntityUIProgressBar : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI label;
        [SerializeField] protected Slider sliderProgress;

        protected EntityMainController entityMainController;
        protected AnimationCurve animationCurve = GameSettings_Battle.Instance?.constantValues.progressUpdateAnimationCurve ?? default;
        protected float UpdateDuration => GameSettings_Battle.Instance.constantValues.progressUpdateDuration;
        protected virtual Color ProgressColor => Color.white;
        GradualChangeValue.Status updateStatus;

        protected abstract float CurrentValue { get; }
        protected abstract float MaxValue { get; }

        public virtual void Init(EntityMainController entityMainController)
        {
            this.entityMainController = entityMainController;
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
            updateStatus = GradualChangeValue.Execute(sliderProgress.value, CurrentValue, UpdateDuration, OnProgressUpdate, animationCurve);
        }

        protected virtual void OnProgressUpdate(GradualChangeValue.Status status)
        {
            if(label)
                SetLabel(status);
            sliderProgress.value = status.CurrentValue;
            if (status.IsDone)
                updateStatus = null;
        }

        protected virtual void SetLabel(GradualChangeValue.Status status) { }
    }
}
