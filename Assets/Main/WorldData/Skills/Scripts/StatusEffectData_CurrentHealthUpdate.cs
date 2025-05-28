using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class StatusEffectData_CurrentHealthUpdate : StatusEffectData
    {
        [SerializeField] bool ignoreCrit;
        [SerializeField] bool ignoreDefense;
        [SerializeField] bool ignoreSheild;

        public override async Task Execute(StatusEffectDataHandler statusEffectDataHandler)
        {
            var executeToEntity = statusEffectDataHandler.ExecuteToEntity;
            var cachedValue = statusEffectDataHandler.cachedValue;

            if (cachedValue < 0)
            {
                if (!ignoreCrit && Random.Range(0, 1f) <= statusEffectDataHandler.caster.BattleAttributes.critChance.Value)
                    cachedValue *= statusEffectDataHandler.caster.BattleAttributes.critDamage.Value;

                if (!ignoreDefense)
                    cachedValue = Mathf.Min(cachedValue + Random.Range(0, executeToEntity.BattleAttributes.defense.Value), 0);

                if (!ignoreSheild)
                {
                    cachedValue = Mathf.Min(cachedValue + executeToEntity.BattleAttributes.sheild.Value, 0);
                    var sheildDamage = Mathf.Max(cachedValue + executeToEntity.BattleAttributes.sheild.Value, 0);
                    executeToEntity.BattleAttributes.sheild.Update(sheildDamage, statusEffectDataHandler);
                }
            }

            executeToEntity.EntityHealthController.AddHealth(cachedValue, this);
            await base.Execute(statusEffectDataHandler);
        }

        public override Task Expire(StatusEffectDataHandler statusEffectDataHandler)
        {
            if (ExpireAction == ExpireActionType.None)
                return base.Expire(statusEffectDataHandler);

            var executeToEntity = statusEffectDataHandler.ExecuteToEntity;
            executeToEntity.BattleAttributes.maxHealth.Remove(statusEffectDataHandler);
            executeToEntity.BattleAttributes.currentHealth.Remove(statusEffectDataHandler);

            return base.Expire(statusEffectDataHandler);
        }
    }
}
