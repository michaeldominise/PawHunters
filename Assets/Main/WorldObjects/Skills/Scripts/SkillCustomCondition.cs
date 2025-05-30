using UnityEngine;

namespace PawHunters
{
    public abstract class SkillCustomCondition : MonoBehaviour
    {
        public virtual bool IsVaild(EntityMainController caster, EntityMainController target) => true;
        public virtual bool IsVaild(EntityMainController caster, object triggerSource) => true;
        public virtual bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource) => true;
    }
}
