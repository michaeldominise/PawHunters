using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class EntityUIHealthBar : EntityUIProgressBar
    {
        Gradient HealthColor => AppSettings_Battle.Instance.colorTheme.healthProgressColor;

        protected override float CurrentValue => entityMainController.BattleAttributes.currentHealth.Value;
        protected override float MaxValue => entityMainController.BattleAttributes.maxHealth.Value;

        private void OnEnable() => BattleManager.Instance.CurrentState.RegisterListener(Refresh);

        public override void Init(EntityMainController entityMainController)
        {
            if (this.entityMainController)
            {
                this.entityMainController.BattleAttributes.maxHealth.OnUpdateValueVoid -= Refresh;
                this.entityMainController.BattleAttributes.currentHealth.OnUpdateValueVoid -= Refresh;
            }

            entityMainController.BattleAttributes.maxHealth.OnUpdateValueVoid += Refresh;
            entityMainController.BattleAttributes.currentHealth.OnUpdateValueVoid += Refresh;

            base.Init(entityMainController);
        }

        protected override void OnProgressUpdate(GradualChangeValue.Status status)
        {
            sliderProgress.image.CrossFadeColor(HealthColor.Evaluate(status.CurrentValue), 0, true, false);
            base.OnProgressUpdate(status);
        }

        protected override void SetLabel(GradualChangeValue.Status status)
        {
            if (entityMainController.BattleAttributes.shield.Value > 0)
                return;
            label.text = GetOverflowValue(status.CurrentValue).Format();
        }
    }
}
