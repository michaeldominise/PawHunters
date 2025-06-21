using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class SkillCustomCondition_StatusEffectTags : SkillCustomCondition
    {
        public enum FlagConditionType { HasFlag, NotHaveFlag }

        [SerializeField] protected StatusEffectTags tags;
        [SerializeField] protected FlagConditionType flagConditionType;

        public override bool IsVaild(EntityMainController caster, object triggerSource)
        {
            if (triggerSource is StatusEffectDataHandler)
                return IsVaild(caster, triggerSource as StatusEffectDataHandler);
            Debug.LogError($"{triggerSource}({triggerSource?.GetType().Name}) is not {nameof(StatusEffectDataHandler)}");
            return true;
        }

        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource)
            => triggerSource.data.Tags.HasFlag(tags) ^ (flagConditionType == FlagConditionType.NotHaveFlag);

        public override bool IsVaild(EntityMainController caster, EntityMainController target)
        {
            Debug.LogError($"{target}({target?.GetType().Name}) is not {nameof(StatusEffectDataHandler)}");
            return true;
        }
    }
}
