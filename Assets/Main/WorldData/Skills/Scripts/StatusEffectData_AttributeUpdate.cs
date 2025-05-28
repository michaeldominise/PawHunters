using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class StatusEffectData_AttributeUpdate : StatusEffectData
    {
        [SerializeField] SkillAttributeData.AttributeType targetAttribute;

        public override async Task Execute(StatusEffectDataHandler statusEffectDataHandler)
        {
            var executeToEntity = statusEffectDataHandler.ExecuteToEntity;
            var cachedValue = statusEffectDataHandler.cachedValue;
            switch (targetAttribute)
            {
                case SkillAttributeData.AttributeType.CurrentHealth:
                    if(cachedValue < 0)
                        Debug.LogError($"This is prohibited to use. Use {nameof(StatusEffectData_CurrentHealthUpdate)}");
                    else
                        executeToEntity.EntityHealthController.AddHealth(cachedValue, statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.MaxHealth:
                    executeToEntity.EntityHealthController.AddMaxHealth(cachedValue, statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Attack:
                    executeToEntity.BattleAttributes.attack.Update(cachedValue, statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Defense:
                    executeToEntity.BattleAttributes.defense.Update(cachedValue, statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Speed:
                    executeToEntity.BattleAttributes.speed.Update(cachedValue, statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.CritChance:
                    executeToEntity.BattleAttributes.critChance.Update(cachedValue, statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.CritDamage:
                    executeToEntity.BattleAttributes.critDamage.Update(cachedValue, statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Sheild:
                    executeToEntity.EntityHealthController.AddSheild(cachedValue, statusEffectDataHandler);
                    break;
                default:
                    break;
            }
            await base.Execute(statusEffectDataHandler);
        }

        public override Task Expire(StatusEffectDataHandler statusEffectDataHandler)
        {
            if (ExpireAction == ExpireActionType.None)
                return base.Expire(statusEffectDataHandler);

            var executeToEntity = statusEffectDataHandler.ExecuteToEntity;

            switch (targetAttribute)
            {
                case SkillAttributeData.AttributeType.MaxHealth:
                    executeToEntity.BattleAttributes.maxHealth.Remove(statusEffectDataHandler);
                    executeToEntity.BattleAttributes.currentHealth.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.CurrentHealth:
                    executeToEntity.BattleAttributes.currentHealth.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Attack:
                    executeToEntity.BattleAttributes.attack.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Defense:
                    executeToEntity.BattleAttributes.defense.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Speed:
                    executeToEntity.BattleAttributes.speed.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.CritChance:
                    executeToEntity.BattleAttributes.critChance.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.CritDamage:
                    executeToEntity.BattleAttributes.critDamage.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Sheild:
                    executeToEntity.BattleAttributes.sheild.Remove(statusEffectDataHandler);
                    break;
            }

            return base.Expire(statusEffectDataHandler);
        }
    }
}
