using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityUISpecialSkillBar : EntityUIProgressBar
    {
        protected override Color ProgressColor => GlobalSettings.Instance.colorTheme.specialSkillProgressColor;
        protected override float SliderCurrentValue => entityMainController.BattleAttributes.specialSkill.Value;

        public override void Init(EntityMainController entityMainController)
        {
            if (this.entityMainController)
                this.entityMainController.BattleAttributes.specialSkill.OnUpdateValueVoid -= Refresh;
            entityMainController.BattleAttributes.specialSkill.OnUpdateValueVoid += Refresh;

            base.Init(entityMainController);
        }
    }
}
