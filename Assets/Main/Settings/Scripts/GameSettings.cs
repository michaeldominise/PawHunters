using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "GlobalSettings", menuName = "GameData/Settings/GlobalSettings")]
    public abstract class GameSettings<EnumType> : ScriptableObject where EnumType : System.Enum
    {
        public abstract class ColorTheme
        {
            public abstract Color GetColor(EnumType enumType);
        }

        public abstract class IconSprite
        {
            public abstract Sprite GetSprite(EnumType enumType);
        }
    }
}
