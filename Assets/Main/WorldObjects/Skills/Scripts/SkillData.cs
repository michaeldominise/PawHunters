using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
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
        public bool playAttackAnimation = true;

        public async Task Execute(GameActionTriggersManager.TriggerType trigger, EntityMainController caster, object triggerSource = null)
        {
            if (!caster.IsAlive
                || !this.trigger.HasFlag(trigger)
                || customConditions.FirstOrDefault(x => !x.IsVaild(caster, triggerSource)))
                return;

            Debug.Log($"{caster.name}:{(bool)caster.TeamManager_GamePlayer} execute Skill:'{title}'");

            if (animationCastType == AnimationCastType.RunToFirstTarget)
                await caster.EntityMovementController.MoveToFront(skillTargets.FirstOrDefault().GetTargetEntities(caster, triggerSource as StatusEffectDataHandler).FirstOrDefault(), true);

            if(playAttackAnimation)
                await PlayAttackAnimation(caster);

            foreach (var skillTarget in skillTargets)
                await skillTarget.Execute(caster);

            if (animationCastType == AnimationCastType.RunToFirstTarget)
                await caster.EntityMovementController.MoveToStartPosition();
        }

        [Button]
        public async Task Execute()
        {
            Debug.Log($"System execute Skill:'{title}'");

            foreach (var skillTarget in skillTargets)
                await skillTarget.Execute(null);
        }

        async Task PlayAttackAnimation(EntityMainController caster)
        {
            var isAttackDone = false;
            void OnAnimationStateUpdate(EntityAnimationController.State state) => isAttackDone = state == EntityAnimationController.State.AttackDone;

            caster.EntityAnimationController.SetState(EntityAnimationController.State.Attacking);
            caster.EntityAnimationController.CurrentState.RegisterListener(OnAnimationStateUpdate);
            while (!isAttackDone)
                await Task.Yield();
            caster.EntityAnimationController.CurrentState.UnregisterListener(OnAnimationStateUpdate);
        }
    }
}
