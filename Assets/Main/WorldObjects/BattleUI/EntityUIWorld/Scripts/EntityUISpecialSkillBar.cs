using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityUISpecialSkillBar : EntityUIProgressBar
    {
        protected override Color ProgressColor => GameSettings_Battle.Instance.colorTheme.specialSkillProgressColor;
        protected override float CurrentValue => entityMainController.BattleAttributes.specialSkill.Value;
        protected override float MaxValue => entityMainController.BattleAttributes.specialSkillMax.Value;

        public override void Init(EntityMainController entityMainController)
        {
            if (this.entityMainController)
                this.entityMainController.BattleAttributes.specialSkill.OnUpdateValueVoid -= Refresh;
            entityMainController.BattleAttributes.specialSkill.OnUpdateValueVoid += Refresh;

            base.Init(entityMainController);
        }
    }
}
