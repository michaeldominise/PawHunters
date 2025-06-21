using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class SkillCustomCondition_ChanceRate : SkillCustomCondition
    {
        [SerializeField, Range(0, 1)] float chanceBaseValue;
        [SerializeField, TableList] List<StatusEffectData.AttributeModifiers> chanceAttributeModifier;

        public override bool IsVaild(EntityMainController caster, object triggerSource)
        {
            if (triggerSource is StatusEffectDataHandler)
                return IsVaild(caster, triggerSource as StatusEffectDataHandler);
            Debug.LogError($"{triggerSource}({triggerSource?.GetType().Name}) is not {nameof(StatusEffectDataHandler)}");
            return true;
        }

        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource) => IsVaild(caster, triggerSource.caster);
        public override bool IsVaild(EntityMainController caster, EntityMainController target)
        {
            var value = chanceBaseValue;
            foreach (var attributeModifier in chanceAttributeModifier)
                value = attributeModifier.GetValue(value, caster, target);
            return Random.value < value;
        }
    }
}
