using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace PawHunters
{
    public class SkillCustomCondition_EntityElements : SkillCustomCondition_StatusEffectElements
    {
        public override bool IsVaild(EntityMainController caster, object triggerSource)
        {
            if (triggerSource is StatusEffectDataHandler)
                return IsVaild(caster, triggerSource as StatusEffectDataHandler);
            Debug.LogError($"{triggerSource}({triggerSource?.GetType().Name}) is not {nameof(StatusEffectDataHandler)}");
            return true;
        }

        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource) => IsVaild(caster, triggerSource.caster);
        public override bool IsVaild(EntityMainController caster, EntityMainController target)
            => target.Element.HasFlag(flagConditionType) ^ flagConditionType == FlagConditionType.NotHaveFlag;
    }
}
