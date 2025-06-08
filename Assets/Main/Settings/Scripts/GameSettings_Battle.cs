using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "GameSettings_Battle", menuName = "GameData/Settings/GameSettings_Battle")]
    public class GameSettings_Battle : GameSettings<GameSettings_Battle.Type>
    {
        public static GameSettings_Battle Instance => SceneGameManager.Instance?.GameSettings_Battle;

        public enum Type
        {
            None,
            Shield,
            Speed,
            Health,
            Attack,
            JourneyDefault,
            JourneyBattle,
            JourneyBoss,
        }

        [System.Serializable]
        public class ColorTheme_Battle : ColorTheme
        {
            public Gradient healthProgressColor;
            public Color shieldProgressColor;
            public Color specialSkillProgressColor;
            public Color criticalColor;
            public Color damageColor;
            public Color healColor;
            public Color journeyFillColor;
            public Color journeyInactiveColor;
            public Color journeyActiveColor_Default;
            public Color journeyActiveColor_Battle;
            public Color journeyActiveColor_Boss;
            public Color blockedColor;
            public ElementColorTheme elementColorOverlay;

            public override Color GetColor(Type enumType)
                => enumType switch
                {
                    Type.JourneyDefault => throw new System.NotImplementedException(),
                    Type.JourneyBattle => throw new System.NotImplementedException(),
                    Type.JourneyBoss => throw new System.NotImplementedException(),
                    _ => Color.clear,
                };
        }

        [System.Serializable]
        public class IconSprite_Battle : IconSprite
        {
            public Sprite shield;
            public Sprite speed;
            public Sprite health;
            public Sprite attack;
            public Sprite journeyDefault;
            public Sprite journeyBattle;
            public Sprite journeyBoss;

            public override Sprite GetSprite(Type enumType)
                => enumType switch
                {
                    Type.Shield => shield,
                    Type.Speed => speed,
                    Type.Health => health,
                    Type.Attack => attack,
                    Type.JourneyDefault => journeyDefault,
                    Type.JourneyBattle => journeyBattle,
                    Type.JourneyBoss => journeyBoss,
                    _ => null,
                };
        }

        [System.Serializable]
        public class ConstantValues
        {
            public float overlayFadeOutDelayDuration;
            public float overlayFadeOutDuration;
            public float titleFadeOutDelayDuration;
            public float titleFadeOutDuration;

            public float progressUpdateDuration;
            public AnimationCurve progressUpdateAnimationCurve;
            public float statusTextUITargetYPosition;
            public float statusTextUITargetScale;
            public float statusTextUILifeDuration;
            public float statusTextUISpawnerRandomAdditionalDistance;
            public AnimationCurve bounceAnimationCurve;
            public float journeyTransitionDuration;
            public float environmentOverlayFadeDuration;
            public float environmentOverlayFadeOpacity;
        }

        public ColorTheme_Battle colorTheme;
        public IconSprite_Battle iconSprite;
        public ConstantValues constantValues;
    }
}
