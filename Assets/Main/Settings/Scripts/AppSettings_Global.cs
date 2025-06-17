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
        }

        [System.Serializable]
        public class IconSprite_Global : IconSprite
        {
        }

        [System.Serializable]
        public class ConstantValues
        {
        }

        public ColorTheme_Global colorTheme;
        public IconSprite_Global iconSprite;
        public ConstantValues constantValues;
    }
}
