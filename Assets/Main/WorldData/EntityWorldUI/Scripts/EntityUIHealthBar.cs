using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityUIHealthBar : MonoBehaviour
    {
        [SerializeField] EntityUIWorld entityUIWorld;
        [SerializeField] Slider healthSlider;
        [SerializeField] Gradient healthColor;
        [SerializeField] float updateDuration = 1;
        [SerializeField] AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public float UpdateDuration => updateDuration;
        EntityMainController entityMainController;
        Coroutine updateHealthCoroutine;

        public void Init(EntityMainController entityMainController)
        {
            if (this.entityMainController)
            {
                this.entityMainController.BattleAttributes.maxHealth.OnUpdateValueVoid -= PlayerHealthController_OnHealthUpdate;
                this.entityMainController.BattleAttributes.currentHealth.OnUpdateValueVoid -= PlayerHealthController_OnHealthUpdate;
                this.entityMainController.CurrentState.UnregisterListener(PlayerMainController_OnStateUpdate);
            }

            this.entityMainController = entityMainController;
            this.entityMainController.BattleAttributes.maxHealth.OnUpdateValueVoid += PlayerHealthController_OnHealthUpdate;
            this.entityMainController.BattleAttributes.currentHealth.OnUpdateValueVoid += PlayerHealthController_OnHealthUpdate;
            this.entityMainController.CurrentState.RegisterListener(PlayerMainController_OnStateUpdate);

            healthSlider.value = 0;
            updateHealthCoroutine = null;
            PlayerHealthController_OnHealthUpdate();
        }

        private void PlayerMainController_OnStateUpdate(EntityMainController.State state)
        {
            if (state == EntityMainController.State.Dead)
                entityUIWorld.Kill();
        }

        void PlayerHealthController_OnHealthUpdate()
        {
            if (updateHealthCoroutine != null || !gameObject.activeInHierarchy)
                return;

            updateHealthCoroutine = GradualChangeValue.Execute(healthSlider.value, entityMainController.BattleAttributes.HealthPercentage, updateDuration,
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
