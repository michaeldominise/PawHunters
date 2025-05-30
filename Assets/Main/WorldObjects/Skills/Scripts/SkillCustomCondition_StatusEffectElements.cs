using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace PawHunters
{
    public class SkillCustomCondition_StatusEffectElements : SkillCustomCondition
    {
        public enum FlagConditionType { HasFlag, NotHaveFlag }

        [SerializeField] protected ElementType elements;
        [SerializeField] protected FlagConditionType flagConditionType;

        public override bool IsVaild(EntityMainController caster, object triggerSource)
        {
            if (triggerSource is StatusEffectDataHandler)
                return IsVaild(caster, triggerSource as StatusEffectDataHandler);
            Debug.LogError($"{triggerSource}({triggerSource?.GetType().Name}) is not {nameof(StatusEffectDataHandler)}");
            return true;
        }

        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource)
            => triggerSource.data.Element.HasFlag(elements) ^ flagConditionType == FlagConditionType.NotHaveFlag;

        public override bool IsVaild(EntityMainController caster, EntityMainController target)
            => target.Element.HasFlag(flagConditionType) ^ flagConditionType == FlagConditionType.NotHaveFlag;
    }
}
