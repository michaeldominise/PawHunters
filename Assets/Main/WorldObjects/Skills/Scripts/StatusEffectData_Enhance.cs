using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class StatusEffectData_Enhance : StatusEffectData
    {
        public enum EnhanceType { Damage, Defense, Heal, Recovery }

        [SerializeField] EnhanceType enhanceType;
        [SerializeField] List<SkillCustomCondition> enhanceCustomConditions;

        public bool CanEnhanceValue(EntityMainController target, EnhanceType enhanceType)
        {
            if (this.enhanceType != enhanceType || enhanceCustomConditions.FirstOrDefault(x => !x.IsVaild(null, target)) != null)
                return false;

            return true;
        }

        public override async Task<float> Execute(StatusEffectDataHandler statusEffectDataHandler)
        {
            await base.Execute(statusEffectDataHandler);
            return statusEffectDataHandler.cachedValue;
        }

        public override Task Expire(StatusEffectDataHandler statusEffectDataHandler) => base.Expire(statusEffectDataHandler);
    }
}
