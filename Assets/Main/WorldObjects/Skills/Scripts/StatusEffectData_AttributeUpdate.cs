using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class StatusEffectData_AttributeUpdate : StatusEffectData
    {
        [SerializeField] SkillAttributeData.AttributeType targetAttribute;

        AppSettings_Global.IconSprites IconSprites => AppSettings_Global.Instance.iconSprites;

        public override async Task<float> Execute(StatusEffectDataHandler statusEffectDataHandler)
        {
            var target = statusEffectDataHandler.target;
            var cachedValue = statusEffectDataHandler.cachedValue;
            switch (targetAttribute)
            {
                case SkillAttributeData.AttributeType.CurrentHealth:
                    if (cachedValue < 0)
                        Debug.LogError($"This is prohibited to use. Use {nameof(StatusEffectData_DoDamage)}");
                    else
                    {
                        cachedValue *= statusEffectDataHandler.caster.EntityStatusEffectController.GetEnhanceValue(statusEffectDataHandler.target, StatusEffectData_Enhance.EnhanceType.Heal);
                        cachedValue *= statusEffectDataHandler.target.EntityStatusEffectController.GetEnhanceValue(statusEffectDataHandler.caster, StatusEffectData_Enhance.EnhanceType.Recovery);
                        target.EntityHealthController.AddHealth(cachedValue, statusEffectDataHandler);
                    }
                    ShowStatusTextUI(target, AppSettings_Battle.Instance.colorTheme.healColor, cachedValue);
                    break;
                case SkillAttributeData.AttributeType.MaxHealth:
                    target.EntityHealthController.AddMaxHealth(cachedValue, statusEffectDataHandler);
                    ShowStatusTextUI(target, Color.white, cachedValue, IconSprites.battleStats.GetSprite(SkillAttributeData.AttributeType.MaxHealth));
                    break;
                case SkillAttributeData.AttributeType.Attack:
                    target.BattleAttributes.attack.Update(cachedValue, statusEffectDataHandler);
                    ShowStatusTextUI(target, Color.white, cachedValue, IconSprites.battleStats.GetSprite(SkillAttributeData.AttributeType.Attack));
                    break;
                case SkillAttributeData.AttributeType.Defense:
                    target.BattleAttributes.defense.Update(cachedValue, statusEffectDataHandler);
                    ShowStatusTextUI(target, Color.white, cachedValue);
                    break;
                case SkillAttributeData.AttributeType.Speed:
                    target.BattleAttributes.speed.Update(cachedValue, statusEffectDataHandler);
                    ShowStatusTextUI(target, Color.white, cachedValue, IconSprites.battleStats.GetSprite(SkillAttributeData.AttributeType.Speed));
                    break;
                case SkillAttributeData.AttributeType.CritChance:
                    target.BattleAttributes.critChance.Update(cachedValue, statusEffectDataHandler);
                    ShowStatusTextUI(target, Color.white, cachedValue);
                    break;
                case SkillAttributeData.AttributeType.CritDamage:
                    target.BattleAttributes.critDamage.Update(cachedValue, statusEffectDataHandler);
                    ShowStatusTextUI(target, Color.white, cachedValue);
                    break;
                case SkillAttributeData.AttributeType.Shield:
                    target.EntityHealthController.AddSheild(cachedValue, statusEffectDataHandler);
                    ShowStatusTextUI(target, Color.white, cachedValue, IconSprites.battleStats.GetSprite(SkillAttributeData.AttributeType.Defense));
                    break;
                case SkillAttributeData.AttributeType.SpecialSkill:
                    target.BattleAttributes.specialSkill.Update(cachedValue, statusEffectDataHandler);
                    ShowStatusTextUI(target, Color.white, cachedValue);
                    break;
                default:
                    break;
            }

            await base.Execute(statusEffectDataHandler);
            return cachedValue;
        }

        public override Task Expire(StatusEffectDataHandler statusEffectDataHandler)
        {
            if (ExpireAction == ExpireActionType.None)
                return base.Expire(statusEffectDataHandler);

            var target = statusEffectDataHandler.target;
            switch (targetAttribute)
            {
                case SkillAttributeData.AttributeType.MaxHealth:
                    target.BattleAttributes.maxHealth.Remove(statusEffectDataHandler);
                    target.BattleAttributes.currentHealth.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.CurrentHealth:
                    target.BattleAttributes.currentHealth.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Attack:
                    target.BattleAttributes.attack.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Defense:
                    target.BattleAttributes.defense.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Speed:
                    target.BattleAttributes.speed.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.CritChance:
                    target.BattleAttributes.critChance.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.CritDamage:
                    target.BattleAttributes.critDamage.Remove(statusEffectDataHandler);
                    break;
                case SkillAttributeData.AttributeType.Shield:
                    target.BattleAttributes.shield.Remove(statusEffectDataHandler);
                    break;
            }

            return base.Expire(statusEffectDataHandler);
        }
    }
}
