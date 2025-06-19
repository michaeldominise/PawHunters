using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "AppSettings_Global", menuName = "GameData/Settings/AppSettings_Global")]
    public class AppSettings_Global : AppSettings
    {
        public static AppSettings_Global Instance => AppManager.Instance?.AppSettings_Global;


        [System.Serializable]
        public class ColorTheme_Global : ColorTheme
        {
            public ElementColorTheme elementColorTheme;
            public RarityColorTheme rarityColorTheme;
            public Color tabSelectedText;
            public Color tabNotSelectedText;
        }

        [System.Serializable]
        public class IconSprite_Global : IconSprite
        {
        }

        [System.Serializable]
        public class ConstantValues
        {
            public List<StateSpeed> movementStateSpeedList = new();

            public float GetSpeed(StateSpeed.State state) => movementStateSpeedList.FirstOrDefault(x => state == x.state)?.speed ?? 0;
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

        public ColorTheme_Global colorTheme;
        public IconSprite_Global iconSprite;
        public ConstantValues constantValues;
        public UIAnimationValues uIAnimationValues;
    }
}
