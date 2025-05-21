using TMPro;
using UnityEngine;

namespace PawHunters
{
    public class MainMenuUI : MonoBehaviour
    {
        public static MainMenuUI Instance;

        [SerializeField] GameObject container;
        [SerializeField] TMP_InputField numberOfPlayers;

        private void Awake() => Instance = this;

        public void Show(bool value) => container.SetActive(value);
        public void StartGame()
        {
            var success = int.TryParse(numberOfPlayers.text, out int spawnCount);
            //PlayerSpawner.Instance.StartGame(success && spawnCount > 0 ? spawnCount : 1);
            Show(false);
        }
    }
}
