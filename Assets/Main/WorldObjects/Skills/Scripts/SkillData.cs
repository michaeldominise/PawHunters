using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace PawHunters
{
    public class SkillData : MonoBehaviour
    {
        public enum AnimationMovementType
        {
            RunToFirstTarget,
            RunBackToStartPosition,
        }

        public enum AnimationAttackType
        {
            None,
            BasicAttack,
            SpecialSkillAttack,
            SpecialSkillCast,
        }

        public string title;
        [TextArea] public string description;
        public Sprite icon;
        public List<SkillTargetData> skillTargets;
        public SkillTargetData runToFirstTarget;
        public GameActionTriggersManager.TriggerType trigger = GameActionTriggersManager.TriggerType.Instant;
        public List<SkillCustomCondition> customConditions;
        public AnimationAttackType playExecuteAnimation;

        public async Task Execute(GameActionTriggersManager.TriggerType trigger, EntityMainController caster, object triggerSource = null)
        {
            if (!caster.IsAlive
                || !this.trigger.HasFlag(trigger)
                || customConditions.FirstOrDefault(x => !x.IsVaild(caster, triggerSource)))
                return;

            Debug.Log($"{caster.name}:{(bool)caster.TeamManager_GamePlayer} execute Skill:'{title}'");

            await PlayMovementAnimation(caster, AnimationMovementType.RunToFirstTarget, triggerSource as StatusEffectDataHandler);
            await PlayAttackAnimation(caster);

            foreach (var skillTarget in skillTargets)
                await skillTarget.Execute(caster);

            await PlayMovementAnimation(caster, AnimationMovementType.RunBackToStartPosition);
        }

        [Button]
        public async Task Execute()
        {
            Debug.Log($"[SkillData.Execute] System execute Skill:'{title}'");

            foreach (var skillTarget in skillTargets)
                await skillTarget.Execute(null);
        }

        async Task PlayMovementAnimation(EntityMainController caster, AnimationMovementType animationMovementType, StatusEffectDataHandler triggerSource = null)
        {
            if (!runToFirstTarget)
                return;

            switch (animationMovementType)
            {
                case AnimationMovementType.RunBackToStartPosition:
                    await caster.EntityMovementController.MoveToStartPosition();
                    break;
                case AnimationMovementType.RunToFirstTarget:
                    await caster.EntityMovementController.MoveToFront(skillTargets.FirstOrDefault().GetTargetEntities(caster, triggerSource).FirstOrDefault(), true);
                    break;
            }
        }

        async Task PlayAttackAnimation(EntityMainController caster)
        {
            if (playExecuteAnimation == AnimationAttackType.None)
                return;

            var isAttackDone = false;
            void OnAnimationStateUpdate(EntityAnimationController.State state) => isAttackDone = state == EntityAnimationController.State.AttackDone;

            caster.EntityAnimationController.SetState(playExecuteAnimation switch
            {
                AnimationAttackType.SpecialSkillAttack => EntityAnimationController.State.SpecialSkillAttack,
                AnimationAttackType.SpecialSkillCast => EntityAnimationController.State.SpecialSkillCast,
                _ => EntityAnimationController.State.BasicAttack
            });
            caster.EntityAnimationController.CurrentState.RegisterListener(OnAnimationStateUpdate);
            while (!isAttackDone)
                await Task.Yield();
            caster.EntityAnimationController.CurrentState.UnregisterListener(OnAnimationStateUpdate);
        }
    }
}
