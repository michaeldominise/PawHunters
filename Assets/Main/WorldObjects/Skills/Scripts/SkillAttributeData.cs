using UnityEngine;

namespace PawHunters
{
    public static class SkillAttributeData
    {
        public enum AttributeType
        {
            MaxHealth,
            CurrentHealth,
            Attack,
            Defense,
            Speed,
            CritChance,
            CritDamage,
            Sheild,
            SpecialSkill,
        }

        public static float GetValue(this AttributeType targetAttribute, EntityMainController entityMainController)
        {
            if (!entityMainController)
                return 0;
            return targetAttribute switch
            {
                AttributeType.MaxHealth => entityMainController.BattleAttributes.maxHealth.Value,
                AttributeType.CurrentHealth => entityMainController.BattleAttributes.currentHealth.Value,
                AttributeType.Attack => entityMainController.BattleAttributes.attack.Value,
                AttributeType.Defense => entityMainController.BattleAttributes.defense.Value,
                AttributeType.Speed => entityMainController.BattleAttributes.speed.Value,
                AttributeType.CritChance => entityMainController.BattleAttributes.critChance.Value,
                AttributeType.CritDamage => entityMainController.BattleAttributes.critDamage.Value,
                AttributeType.Sheild => entityMainController.BattleAttributes.shield.Value,
                AttributeType.SpecialSkill => entityMainController.BattleAttributes.specialSkill.Value,
                _ => 0,
            };
        }
    }
}
