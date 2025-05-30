using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace PawHunters
{
    public class SkillData : MonoBehaviour
    {
        public enum AnimationCastType
        {
            InPlace,
            RunToFirstTarget,
        }


        public string title;
        [TextArea] public string description;
        public Sprite icon;
        public List<SkillTargetData> skillTargets;
        public bool runToTargetBeforeExecute;
        public GameActionTriggersManager.TriggerType trigger = GameActionTriggersManager.TriggerType.Instant;
        public List<SkillCustomCondition> customConditions;
        public AnimationCastType animationCastType;

        public async Task Execute(GameActionTriggersManager.TriggerType trigger, EntityMainController caster, object triggerSource = null)
        {
            if (!caster.IsAlive
                || !this.trigger.HasFlag(trigger)
                || customConditions.FirstOrDefault(x => !x.IsVaild(caster, triggerSource)))
                return;

            Debug.Log($"{caster.name}:{(bool)caster.TeamManager_GamePlayer} execute Skill:'{title}'");
            var firstTarget = skillTargets.FirstOrDefault().GetTargetEntities(caster, triggerSource as StatusEffectDataHandler).FirstOrDefault();
            if (animationCastType == AnimationCastType.RunToFirstTarget)
                await caster.EntityMovementController.MoveToFront(firstTarget, true);

            foreach (var skillTarget in skillTargets)
                await skillTarget.Execute(caster);

            if (animationCastType == AnimationCastType.RunToFirstTarget)
                await caster.EntityMovementController.MoveToStartPosition();
        }
    }
}
