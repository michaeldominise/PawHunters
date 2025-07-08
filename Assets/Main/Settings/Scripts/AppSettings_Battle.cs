using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "AppSettings_Battle", menuName = "GameData/Settings/AppSettings_Battle")]
    public class AppSettings_Battle : AppSettings
    {
        public static AppSettings_Battle Instance => SceneGameManager.Instance?.AppSettings_Battle;

        [System.Serializable]
        public class ColorTheme
        {
            public Gradient healthProgressColor;
            public Color shieldProgressColor;
            public Color specialSkillProgressColor;
            public Color expProgressColor;

            public Color criticalColor;
            public Color damageColor;
            public Color healColor;
            public Color blockedColor;

            public Color journeyFillColor;
            public Color journeyInactiveColor;
            public Color journeyActiveColor_Default;
            public Color journeyActiveColor_Battle;
            public Color journeyActiveColor_Boss;

            public Color journeyLogBattleColor;
            public Color journeyLogBossColor;
            public Color journeyLogNegativeColor;
            public Color journeyLogPositiveColor;
            public Color journeyLogRewardColor;
        }

        [System.Serializable]
        public class IconSprites
        {
            public enum Type
            {
                None,
                JourneyDefault,
                JourneyBattle,
                JourneyBoss,
            }

            public Sprite journeyDefault;
            public Sprite journeyBattle;
            public Sprite journeyBoss;

            public Sprite GetSprite(Type enumType)
                => enumType switch
                {
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
            public float progressUpdateDurationSlow;
            public AnimationCurve progressUpdateAnimationCurve;
            public float statusTextUITargetYPosition;
            public float statusTextUITargetScale;
            public float statusTextUILifeDuration;
            public float statusTextUISpawnerRandomAdditionalDistance;
            public AnimationCurve bounceAnimationCurve;
            public float journeyTransitionDuration;

            public float environmentOverlayFadeDuration;
            public float environmentOverlayFadeOpacity;

            public float journeyLogsTransitionDuration;
            public float expMaxValue;
        }

        public ColorTheme colorTheme;
        [FormerlySerializedAs("iconSprite")]
        public IconSprites iconSprites;
        public ConstantValues constantValues;
    }

    [System.Serializable]
    public class StateSpeed
    {
        public enum State { Idle, Walking, Running }

        public State state;
        public float speed;
    }
}
