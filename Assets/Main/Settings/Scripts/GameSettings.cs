using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "GlobalSettings", menuName = "GameData/Settings/GlobalSettings")]
    public abstract class GameSettings : ScriptableObject
    {
        public abstract class ColorTheme { }
        public abstract class IconSprite { }
    }
}
