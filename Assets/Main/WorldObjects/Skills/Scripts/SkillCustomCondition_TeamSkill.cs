using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class SkillCustomCondition_TeamSkill : SkillCustomCondition
    {
        public override bool IsVaild(EntityMainController caster, object triggerSource)
        {
            var value = caster is EntityMainController_Team;
            if(!value)
                Debug.LogError($"{caster}({caster?.GetType().Name}) is not {nameof(EntityMainController_Team)}");
            return value;
        }
        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource) => IsVaild(caster, (object)triggerSource);
        public override bool IsVaild(EntityMainController caster, EntityMainController target) => IsVaild(caster, (object)target);
    }
}
