using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "GlobalSettings", menuName = "GameData/Settings/GlobalSettings")]
    public class GlobalSettings : ScriptableObject
    {
        public static GlobalSettings Instance => GameManager.Instance?.GlobalSettings;

        [System.Serializable]
        public class ColorTheme
        {
            public Gradient healthProgressColor;
            public Color shieldProgressColor;
            public Color specialSkillProgressColor;
            public Color criticalColor;
            public Color damageColor;
            public Color healColor;
        }

        [System.Serializable]
        public class GameSettings
        {
            public float progressUpdateDuration;
            public AnimationCurve progressUpdateAnimationCurve;
            public float statusTextUITargetYPosition;
            public float statusTextUITargetScale;
            public float statusTextUILifeDuration;
            public float statusTextUISpawnerRandomAdditionalDistance;
            public AnimationCurve bounceAnimationCurve;
        }

        public ColorTheme colorTheme;
        public GameSettings gameSettings;
    }
}
