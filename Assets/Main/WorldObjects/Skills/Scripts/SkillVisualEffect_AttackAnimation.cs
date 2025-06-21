using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class SkillVisualEffect_AttackAnimation : SkillVisualEffect
    {
        public enum AnimationAttackType
        {
            None,
            BasicAttack,
            SpecialSkillAttack,
            SpecialSkillCast,
        }
        public AnimationAttackType playExecuteAnimation;

        public override async Task PlayStart(EntityMainController caster, params EntityMainController[] targets) => await PlayAttackAnimation(caster);
        public override async Task PlayEnd(EntityMainController caster, params EntityMainController[] targets) => await PlayAttackAnimation(caster);

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
