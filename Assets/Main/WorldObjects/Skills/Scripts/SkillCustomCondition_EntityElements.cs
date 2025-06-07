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
            Debug.LogError($"{triggerSource}({triggerSource?.GetType().Name}) is not {nameof(EntityMainController)}");
            return true;
        }

        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource) => IsVaild(caster, triggerSource as object);
        public override bool IsVaild(EntityMainController caster, EntityMainController target) => target.Element.HasFlag(elementFilters) ^ flagConditionType == FlagConditionType.Exclude;
    }
}
