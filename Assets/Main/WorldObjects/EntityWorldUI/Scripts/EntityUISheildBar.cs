using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityUISheildBar : EntityUIProgressBar
    {
        protected override Color ProgressColor => GlobalSettings.Instance.colorTheme.shieldProgressColor;

        protected override float SliderCurrentValue => Mathf.Min(entityMainController.BattleAttributes.shield.Value / entityMainController.BattleAttributes.currentHealth.Value, 1);

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
            label.text = Mathf.Lerp(0, entityMainController.BattleAttributes.shield.Value, status.CurrentValue).Format();
        }
    }
}
