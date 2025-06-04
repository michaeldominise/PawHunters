using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "GameSettings_Global", menuName = "GameData/Settings/GameSettings_Global")]
    public class GameSettings_Global : ScriptableObject
    //public class GameSettings_Global : GameSettings<GameSettings_Global.Type>
    {
        public enum Type { }

        public static GameSettings_Global Instance => GameManager.Instance?.GameSettings_Global;
    }
}
