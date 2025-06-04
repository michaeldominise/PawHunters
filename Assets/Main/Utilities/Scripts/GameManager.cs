using UnityEngine;

namespace PawHunters
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] GameSettings_Global gameSettings_Global;
        public GameSettings_Global GameSettings_Global => gameSettings_Global;

        private void Awake() => Instance = this;
        private void Update() => SaveableData.Execute();
    }
}
