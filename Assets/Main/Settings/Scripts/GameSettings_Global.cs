using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "GameSettings_Global", menuName = "GameData/Settings/GameSettings_Global")]
    public class GameSettings_Global : GameSettings
    {
        public static GameSettings_Global Instance => GameManager.Instance?.GameSettings_Global;


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
