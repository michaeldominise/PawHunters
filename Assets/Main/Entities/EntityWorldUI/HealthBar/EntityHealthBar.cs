using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityHealthBar : MonoBehaviour
    {
        [SerializeField] EntityWorldUI entityWorldUI;
        [SerializeField] Slider healthSlider;
        [SerializeField] Gradient healthColor;
        [SerializeField] float updateDuration = 1;
        [SerializeField] AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public float UpdateDuration => updateDuration;
        EntityMainController EntityMainController => entityWorldUI.EntityMainController;
        EntityHealthController PlayerHealthController => EntityMainController.EntityHealthController;
        Coroutine updateHealthCoroutine;

        public void Init()
        {
            if (this.EntityMainController)
            {
                this.EntityMainController.EntityHealthController.OnHealthUpdate -= PlayerHealthController_OnHealthUpdate;
                this.EntityMainController.CurrentState.OnStateUpdate -= PlayerMainController_OnStateUpdate;
            }

            this.EntityMainController.EntityHealthController.OnHealthUpdate += PlayerHealthController_OnHealthUpdate;
            this.EntityMainController.CurrentState.OnStateUpdate += PlayerMainController_OnStateUpdate;

            healthSlider.value = 0;
            updateHealthCoroutine = null;
            PlayerHealthController_OnHealthUpdate();
        }

        private void PlayerMainController_OnStateUpdate(EntityMainController.State state)
        {
            if (state == EntityMainController.State.Dead)
                entityWorldUI.Kill();
        }

        void PlayerHealthController_OnHealthUpdate()
        {
            if (updateHealthCoroutine != null || !gameObject.activeInHierarchy)
                return;

            updateHealthCoroutine = GradualChangeValue.Execute(healthSlider.value, PlayerHealthController.HealthProgress, updateDuration,
                status =>
                {
                    healthSlider.value = status.CurrentValue;
                    healthSlider.image.CrossFadeColor(healthColor.Evaluate(healthSlider.value), 0, true, false);
                    if (status.IsDone)
                        updateHealthCoroutine = null;
                }, animationCurve);
        }
    }
}
