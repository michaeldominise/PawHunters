using UnityEngine;

namespace PawHunters
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [SerializeField] GameSettings_Global gameSettings_Global;
        public GameSettings_Global GameSettings_Global => gameSettings_Global;

        private void Update() => SaveableData.Execute();
    }
}
