using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class StatusEffectData_DoDamage : StatusEffectData
    {
        [SerializeField] bool ignoreCrit;
        [SerializeField] bool ignoreDefense;
        [SerializeField] bool ignoreSheild;

        public override async Task<float> Execute(StatusEffectDataHandler statusEffectDataHandler)
        {
            var target = statusEffectDataHandler.target;
            var cachedValue = statusEffectDataHandler.cachedValue;
            var colorLabel = GlobalSettings.Instance.colorTheme.damageColor;

            if (cachedValue < 0)
            {
                if (!ignoreCrit && Random.Range(0, 1f) <= statusEffectDataHandler.caster.BattleAttributes.critChance.Value)
                {
                    colorLabel = GlobalSettings.Instance.colorTheme.criticalColor;
                    cachedValue *= statusEffectDataHandler.caster.BattleAttributes.critDamage.Value;
                }

                if (!ignoreDefense)
                    cachedValue = Mathf.Min(cachedValue + Random.Range(0, target.BattleAttributes.defense.Value), 0);

                if (!ignoreSheild)
                {
                    cachedValue = Mathf.Min(cachedValue + target.BattleAttributes.shield.Value, 0);
                    var shieldDamage = Mathf.Max(cachedValue + target.BattleAttributes.shield.Value, 0);
                    target.BattleAttributes.shield.Update(shieldDamage, statusEffectDataHandler);
                }
            }

            target.EntityHealthController.AddHealth(cachedValue, this);
            StatusTextUISpawner.Instance.Spawn(target.Anchor.statusTextUI.position, colorLabel, cachedValue);
            await base.Execute(statusEffectDataHandler);
            return cachedValue;
        }

        public override Task Expire(StatusEffectDataHandler statusEffectDataHandler)
        {
            if (ExpireAction == ExpireActionType.None)
                return base.Expire(statusEffectDataHandler);

            var target = statusEffectDataHandler.target;
            target.BattleAttributes.maxHealth.Remove(statusEffectDataHandler);
            target.BattleAttributes.currentHealth.Remove(statusEffectDataHandler);

            return base.Expire(statusEffectDataHandler);
        }
    }
}
