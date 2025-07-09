using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "AppSettings_Global", menuName = "GameData/Settings/AppSettings_Global")]
    public class AppSettings_Global : AppSettings
    {
        public static AppSettings_Global Instance => AppManager.Instance?.AppSettings_Global;


        [System.Serializable]
        public class ColorTheme
        {
            public ElementColorTheme elementColorTheme;
            public ElementColorTheme elementColorTheme_Background;
            public RarityColorTheme rarityColorTheme;
            public RarityColorTheme rarityColorTheme_Background;
            public Color tabSelectedText;
            public Color tabNotSelectedText;
            public Color tabNotSelectedText_PlayerTeam;
        }

        [System.Serializable]
        public class IconSprites
        {
            [System.Serializable]
            public class BattleStats
            {
                public Sprite defense;
                public Sprite speed;
                public Sprite health;
                public Sprite attack;
                public Sprite critChance;
                public Sprite critDamage;

                public Sprite GetSprite(SkillAttributeData.AttributeType enumType)
                    => enumType switch
                    {
                        SkillAttributeData.AttributeType.Defense
                            or SkillAttributeData.AttributeType.Shield => defense,
                        SkillAttributeData.AttributeType.Speed => speed,
                        SkillAttributeData.AttributeType.CurrentHealth
                            or SkillAttributeData.AttributeType.MaxHealth => health,
                        SkillAttributeData.AttributeType.Attack => attack,
                        SkillAttributeData.AttributeType.CritChance => critChance,
                        SkillAttributeData.AttributeType.CritDamage => critDamage,
                        _ => null,
                    };
            }

            [System.Serializable]
            public class Element
            {
                public Sprite normal;
                public Sprite water;
                public Sprite fire;
                public Sprite electric;
                public Sprite dark;
                public Sprite fighter;
                public Sprite bug;
                public Sprite dragon;
                public Sprite fairy;
                public Sprite flying;
                public Sprite ghost;
                public Sprite grass;
                public Sprite ground;
                public Sprite ice;
                public Sprite poison;
                public Sprite psychic;
                public Sprite rock;
                public Sprite steel;

                public Sprite GetSprite(ElementType enumType)
                 => enumType switch
                 {
                     ElementType.Normal => normal,
                     ElementType.Water => water,
                     ElementType.Fire => fire,
                     ElementType.Electric => electric,
                     ElementType.Dark => dark,
                     ElementType.Fighter => fighter,
                     ElementType.Bug => bug,
                     ElementType.Dragon => dragon,
                     ElementType.Fairy => fairy,
                     ElementType.Flying => flying,
                     ElementType.Ghost => ghost,
                     ElementType.Grass => grass,
                     ElementType.Ground => ground,
                     ElementType.Ice => ice,
                     ElementType.Poison => poison,
                     ElementType.Psychic => psychic,
                     ElementType.Rock => rock,
                     ElementType.Steel => steel,
                     _ => normal,
                 };
            }

            public BattleStats battleStats;
            public Element element;
        }

        [System.Serializable]
        public class ConstantValues
        {
            [System.Serializable]
            public class BattleStatsMinMax
            {
                public Vector2 defense = new(10f, 25f);
                public Vector2 speed = new(45f, 95f);
                public Vector2 health = new(150f, 300f);
                public Vector2 attack = new(50f, 90f);
                public Vector2 critChance = new(0.05f, 0.25f);
                public Vector2 critDamage = new(1.4f, 2.0f);

                public float GetNormalizedPercentatge(float value, SkillAttributeData.AttributeType enumType)
                    => enumType switch
                    {
                        SkillAttributeData.AttributeType.MaxHealth
                            or SkillAttributeData.AttributeType.CurrentHealth => Mathf.Clamp01((value - health.x) / (health.y - health.x)),
                        SkillAttributeData.AttributeType.Attack => Mathf.Clamp01((value - attack.x) / (attack.y - attack.x)),
                        SkillAttributeData.AttributeType.Defense
                            or SkillAttributeData.AttributeType.Shield => Mathf.Clamp01((value - speed.x) / (speed.y - speed.x)),
                        SkillAttributeData.AttributeType.Speed => Mathf.Clamp01((value - speed.x) / (speed.y - speed.x)),
                        SkillAttributeData.AttributeType.CritChance => Mathf.Clamp01((value - critChance.x) / (critChance.y - critChance.x)),
                        SkillAttributeData.AttributeType.CritDamage => Mathf.Clamp01((value - critDamage.x) / (critDamage.y - critDamage.x)),
                        _ => 0,
                    };
            }

            public List<StateSpeed> movementStateSpeedList = new();
            public BattleStatsMinMax battleStatsMinMax = new();
            public float holdDuration = 1f;
            public float leveledGrowthRateValue = 1.1f;

            public float GetSpeed(StateSpeed.State state) => movementStateSpeedList.FirstOrDefault(x => state == x.state)?.speed ?? 0;
            public float LeveledMultiplier(int level) => Mathf.Pow(leveledGrowthRateValue, level - 1);
        }

        [System.Serializable]
        public class UIAnimationValues
        {
            public AnimationCurve genericAnimationCurve;
            public float effectDuration = 0.25f;
            public float scaleNormal = 1;
            public float scaleUp = 1.25f;
            public float scaleDown = 0.75f;
        }

        public ColorTheme colorTheme;
        [FormerlySerializedAs("iconSprite")]
        public IconSprites iconSprites;
        public ConstantValues constantValues;
        public UIAnimationValues uIAnimationValues;
    }
}
