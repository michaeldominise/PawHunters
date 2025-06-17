using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
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
            var colorLabel = AppSettings_Battle.Instance.colorTheme.damageColor;

            if (cachedValue < 0)
            {
                cachedValue *= statusEffectDataHandler.caster.EntityStatusEffectController.GetEnhanceValue(statusEffectDataHandler.target, StatusEffectData_Enhance.EnhanceType.Damage);

                if (!ignoreCrit && Random.Range(0, 1f) <= statusEffectDataHandler.caster.BattleAttributes.critChance.Value)
                {
                    colorLabel = AppSettings_Battle.Instance.colorTheme.criticalColor;
                    cachedValue *= statusEffectDataHandler.caster.BattleAttributes.critDamage.Value;
                }

                if (!ignoreDefense)
                {
                    var totalDefense = target.BattleAttributes.defense.Value + statusEffectDataHandler.target.EntityStatusEffectController.GetEnhanceValue(statusEffectDataHandler.caster, StatusEffectData_Enhance.EnhanceType.Defense);
                    cachedValue = Mathf.Min(cachedValue + Random.Range(0, totalDefense), 0);
                    if(cachedValue == 0)
                        StatusTextUISpawner.Instance.Spawn(target.Anchor.statusTextUI.position, AppSettings_Battle.Instance.colorTheme.blockedColor, "block");
                }

                if (!ignoreSheild && target.BattleAttributes.shield.Value > 0)
                {
                    var shieldDamage = Mathf.Max(cachedValue, -target.BattleAttributes.shield.Value);
                    if (target.BattleAttributes.shield.Value + cachedValue < 1)
                    {
                        shieldDamage = -target.BattleAttributes.shield.Value;
                        StatusTextUISpawner.Instance.Spawn(target.Anchor.statusTextUI.position, AppSettings_Battle.Instance.colorTheme.shieldProgressColor, "break");
                        cachedValue = 0;
                    }
                    target.BattleAttributes.shield.Update(shieldDamage, statusEffectDataHandler);
                }
                ShowStatusTextUI(target, colorLabel, cachedValue);
            }

            target.EntityHealthController.AddHealth(cachedValue, this);
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
