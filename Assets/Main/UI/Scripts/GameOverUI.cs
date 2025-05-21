using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace PawHunters
{
    public class GameOverUI : MonoBehaviour
    {
        public static GameOverUI Instance;

        [SerializeField] GameObject container;
        [SerializeField] TextMeshProUGUI winnerText;

        private void Awake() => Instance = this;
        //private void Start() => PlayerSpawner.Instance.OnSpawned += Spawn;

        public void Show(bool value) => container.SetActive(value);
        public void GameOver()
        {
            Show(true);

            //var winner = PlayerSpawner.Instance.Spawnedlist.FirstOrDefault(x => x.CurrentState.Value != EntityMainController.State.Dead);
            //winnerText.text = winner ? $"{winner.gameObject.name} won the game!" : "All players have been killed";
        }

        public void ShowMainMenu()
        {
            //var winner = PlayerSpawner.Instance.Spawnedlist.FirstOrDefault(x => x.CurrentState != EntityMainController.State.Dead);
            //if (winner)
            //    winner.Kill();

            MainMenuUI.Instance.Show(true);
            Show(false);
        }

        private void Spawn(EntityMainController player)
        {
            //player.OnStateUpdate -= Player_OnStateUpdate;
            //player.OnStateUpdate += Player_OnStateUpdate;
        }

        private void Player_OnStateUpdate(EntityMainController.State state)
        {
            switch (state)
            {
                case EntityMainController.State.Dead:
                    if (HasWinner())
                        GameOver();
                    break;
            }
        }

        bool HasWinner() => false;
            //PlayerSpawner.Instance.Spawnedlist.FindAll(x => x.CurrentState.Value != EntityMainController.State.Dead).Count <= 1;
    }
}
