using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class SkillVisualEffect : MonoBehaviour
    {
        [System.Flags]
        public enum State
        {
            None,
            Start = 1 << 0,
            End = 1 << 1,
            All = (1 << 2) - 1, 
        }

        [SerializeField] State playableStates = State.All;

        [Button]
        public async Task Play(State state, EntityMainController caster, params EntityMainController[] targets)
        {
            if(playableStates.HasFlag(State.Start) && state.HasFlag(State.Start))
                await PlayStart(caster, targets);
            if(playableStates.HasFlag(State.End) && state.HasFlag(State.End))
                await PlayEnd(caster, targets);
        }

        public virtual async Task PlayStart(EntityMainController caster, params EntityMainController[] targets) => await Task.Yield();
        public virtual async Task PlayEnd(EntityMainController caster, params EntityMainController[] targets) => await Task.Yield();

        public static async Task PlayVisual(State state, IEnumerable<SkillVisualEffect> skillVFXs, StatusEffectDataHandler statusEffectDataHandler)
            => await PlayVisual(state, skillVFXs, statusEffectDataHandler.caster, statusEffectDataHandler.target);

        public static async Task PlayVisual(State state, IEnumerable<SkillVisualEffect> skillVFXs, EntityMainController caster, params EntityMainController[] targets)
        {
            foreach (var skillVFX in skillVFXs)
                await skillVFX.Play(state, caster, targets);
        }
    }
}
