using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityWorldUI : MonoBehaviour
    {
        [SerializeField] EntityMainController entityMainController;
        [SerializeField] Slider healthSlider;
        [SerializeField] Gradient healthColor;
        [SerializeField] TextMeshProUGUI playerName;
        [SerializeField] float updateDuration = 1;
        [SerializeField] Camera worldCamera;

        RectTransform RectParent => transform.parent as RectTransform;
        EntityHealthController PlayerHealthController => entityMainController.EntityHealthController;
        Coroutine updateHealthCoroutine;
        

        public void Init(EntityMainController entityMainController, Camera worldCamera)
        {
            gameObject.SetActive(true);
            this.worldCamera = worldCamera;

            if (this.entityMainController)
            {
                this.entityMainController.EntityHealthController.OnHealthUpdate -= PlayerHealthController_OnHealthUpdate;
                this.entityMainController.OnStateUpdate -= PlayerMainController_OnStateUpdate;
            }

            this.entityMainController = entityMainController;
            this.entityMainController.EntityHealthController.OnHealthUpdate += PlayerHealthController_OnHealthUpdate;
            this.entityMainController.OnStateUpdate += PlayerMainController_OnStateUpdate;

            playerName.text = entityMainController.gameObject.name;
            healthSlider.value = 0;
            updateHealthCoroutine = null;
            UpdateHealth();
        }

        private void PlayerMainController_OnStateUpdate(EntityMainController.State state)
        {
            if (state == EntityMainController.State.Dead)
                gameObject.SetActive(false);
        }

        private void PlayerHealthController_OnHealthUpdate(int value) => UpdateHealth();

        void UpdateHealth()
        {
            if (updateHealthCoroutine != null || !gameObject.activeInHierarchy)
                return;
            updateHealthCoroutine = GradualChangeValue.Execute(healthSlider.value, PlayerHealthController.HealthProgress, updateDuration,
                status =>
                {
                    healthSlider.value = status.Progress;
                    healthSlider.image.CrossFadeColor(healthColor.Evaluate(healthSlider.value), 0, true, false);
                });
        }

        private void LateUpdate()
        {
            var viewportPoint = worldCamera.WorldToViewportPoint(entityMainController.WorldUIPoint.position);
            var halfScreenSize = new Vector3(RectParent.rect.width, RectParent.rect.height) * 0.5f;
            transform.localPosition = new Vector3(Mathf.Lerp(-halfScreenSize.x, halfScreenSize.x, viewportPoint.x), Mathf.Lerp(-halfScreenSize.y, halfScreenSize.y, viewportPoint.y));
        }
    }
}
