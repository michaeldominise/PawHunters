using System;
using UnityEngine;

namespace PawHunters
{
    [System.Serializable]
    public class BattleAttributes
    {
        public RecordedFloat maxHealth = new();
        public RecordedFloat currentHealth = new();
        public RecordedFloat attack = new();
        public RecordedFloat defense = new();
        public RecordedFloat speed = new();
        public RecordedSheild shield = new();
        public RecordedFloat specialSkillMax = new();
        public RecordedFloatClamped specialSkill = new();
        public RecordedFloat critChance = new();
        public RecordedFloat critDamage = new();
        public RecordedFloat counterChance = new();
        public RecordedFloat comboChance = new();
        public RecordedFloat dodgeChance = new();
        public RecordedFloat stunned = new();
        public float HealthPercentage => maxHealth.Value == 0 ? 0 : currentHealth.Value / maxHealth.Value;

        public BattleAttributes() { }
        public BattleAttributes(SaveableObjectAttributeData.Attribute attribute) => Init(attribute);

        internal void Init(SaveableObjectAttributeData.Attribute attribute)
        {
            maxHealth.Reset(attribute.health);
            currentHealth.Reset(attribute.health);
            attack.Reset(attribute.attack);
            defense.Reset(attribute.defense);
            speed.Reset(attribute.speed);
            critChance.Reset(attribute.critChance);
            critDamage.Reset(attribute.critDamage);
            specialSkillMax.Reset(attribute.specialSkillMax);
            specialSkill.Reset(() => 0f, () => attribute.specialSkillMax);
            shield.Reset(0);
            stunned.Reset(0);
        }
    }
}
