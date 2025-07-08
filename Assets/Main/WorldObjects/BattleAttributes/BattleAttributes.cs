using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public interface IAttribute
    {
        public Attribute Attribute { get; }
    }

    [Serializable]
    public class Attribute
    {
        public int health = 100;
        public int attack = 10;
        public int defense = 2;
        public int speed = 3;
        public float critChance = 0.1f;
        public float critDamage = 1.2f;
        public float specialSkillMax;
    }

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
        public RecordedFloat immobilize = new();
        public float HealthPercentage => maxHealth.Value == 0 ? 0 : currentHealth.Value / maxHealth.Value;

        public BattleAttributes() { }
        public BattleAttributes(Attribute attribute) => Init(attribute);

        internal void Init(Attribute attribute)
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
            immobilize.Reset(0);
        }

        public void Reset()
        {
            maxHealth.Reset(0);
            currentHealth.Reset(0);
            attack.Reset(0);
            defense.Reset(0);
            speed.Reset(0);
            critChance.Reset(0);
            critDamage.Reset(0);
            specialSkillMax.Reset(0);
            specialSkill.Reset(0);
            shield.Reset(0);
            immobilize.Reset(0);
        }
    }
}
