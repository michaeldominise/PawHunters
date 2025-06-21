using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class SkillCustomCondition_StatusEffectElements : SkillCustomCondition
    {
        public enum FlagConditionType { Include, Exclude }

        [SerializeField] protected FlagConditionType flagConditionType;
        [SerializeField] protected ElementType elementFilters;

        public override bool IsVaild(EntityMainController caster, object triggerSource)
        {
            if (triggerSource is StatusEffectDataHandler)
                return IsVaild(caster, triggerSource as StatusEffectDataHandler);
            Debug.LogError($"{triggerSource}({triggerSource?.GetType().Name}) is not {nameof(StatusEffectDataHandler)}");
            return true;
        }

        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource)
            => triggerSource.data.Element.HasFlag(elementFilters) ^ flagConditionType == FlagConditionType.Exclude;

        public override bool IsVaild(EntityMainController caster, EntityMainController target)
        {
            Debug.LogError($"{target}({target?.GetType().Name}) is not {nameof(StatusEffectDataHandler)}");
            return true;
        }
    }
}
