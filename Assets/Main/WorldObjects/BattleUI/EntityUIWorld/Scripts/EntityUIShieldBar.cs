using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class EntityUIShieldBar : EntityUIProgressBar
    {
        protected override Color ProgressColor => AppSettings_Battle.Instance.colorTheme.shieldProgressColor;

        protected override float CurrentValue => entityMainController.BattleAttributes.shield.Value;
        protected override float MaxValue => entityMainController.BattleAttributes.maxHealth.Value;

        public override void Init(EntityMainController entityMainController)
        {
            if (this.entityMainController)
            {
                this.entityMainController.BattleAttributes.shield.OnUpdateValueVoid -= Refresh;
                this.entityMainController.BattleAttributes.currentHealth.OnUpdateValueVoid -= Refresh;
            }

            entityMainController.BattleAttributes.shield.OnUpdateValueVoid += Refresh;
            entityMainController.BattleAttributes.currentHealth.OnUpdateValueVoid += Refresh;

            base.Init(entityMainController);
        }

        protected override void SetLabel(GradualChangeValue.Status status)
        {
            if (entityMainController.BattleAttributes.shield.Value <= 0)
                return;

            if (status.CurrentValue <= 0)
                label.text = $"{GetOverflowValue(entityMainController.BattleAttributes.currentHealth.Value).Format()}";
            else
                label.text = $"({GetOverflowValue(status.CurrentValue).Format()})";
        }
    }
}
