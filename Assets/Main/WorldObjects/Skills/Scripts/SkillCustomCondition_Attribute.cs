using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class SkillCustomCondition_Attribute : SkillCustomCondition
    {
        public enum OperatorType { Equal, NotEqual, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual, }

        [SerializeField] float baseValueA;
        [SerializeField, TableList] List<StatusEffectData.AttributeModifiers> attributeModifierA;
        [SerializeField] OperatorType operatorType;
        [SerializeField] float baseValueB;
        [SerializeField, TableList] List<StatusEffectData.AttributeModifiers> attributeModifierB;

        public override bool IsVaild(EntityMainController caster, object triggerSource) => IsVaild(caster, caster);
        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource) => IsVaild(caster, triggerSource.caster);
        public override bool IsVaild(EntityMainController caster, EntityMainController target)
        {
            var valueA = GetValue(baseValueA, attributeModifierA, caster, target);
            var valueB = GetValue(baseValueB, attributeModifierB, caster, target);

            return operatorType switch
            {
                OperatorType.Equal => valueA == valueB,
                OperatorType.NotEqual => valueA != valueB,
                OperatorType.GreaterThan => valueA > valueB,
                OperatorType.LessThan => valueA < valueB,
                OperatorType.GreaterThanOrEqual => valueA >= valueB,
                OperatorType.LessThanOrEqual => valueA <= valueB,
                _ => true
            };
        }

        float GetValue(float baseValue, List<StatusEffectData.AttributeModifiers> attributeModifiers, EntityMainController caster, EntityMainController target)
        {
            var value = baseValue;
            foreach (var attributeModifier in attributeModifiers)
                value = attributeModifier.GetValue(value, caster, target);
            return value;
        }
    }
}
