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
        public RecordedFloat shield = new();
        public RecordedFloat specialSkillMax = new();
        public RecordedFloat specialSkill = new();
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
            maxHealth.Update(attribute.health);
            currentHealth.Update(attribute.health);
            attack.Update(attribute.attack);
            defense.Update(attribute.defense);
            speed.Update(attribute.speed);
            critChance.Update(attribute.critChance);
            critDamage.Update(attribute.critDamage);
            specialSkillMax.Update(attribute.specialSkillMax);

            shield.Update(0);
            specialSkill.Update(0);
            stunned.Update(0);
        }
    }
}
