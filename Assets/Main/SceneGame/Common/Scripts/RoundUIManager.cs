using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace PawHunters
{
    public class RoundUIManager : MonoBehaviour
    {
        public static RoundUIManager Instance { get; private set; }

        [SerializeField] GameObject container;
        [SerializeField] TextMeshProUGUI roundLabel;

        int MaxRound => SceneGameManager.Instance.LevelData.maxRound;

        private void Awake() => Instance = this;

        private IEnumerator Start()
        {
            yield return null;
            BattleManager.Instance.CurrentState.RegisterListener(BattleManager_CurrentStateUpdate);
        }

        private void OnDestroy() => BattleManager.Instance.CurrentState.UnregisterListener(BattleManager_CurrentStateUpdate);

        private void BattleManager_CurrentStateUpdate(BattleManager.State state)
        {
            switch (state)
            {
                case BattleManager.State.StartBattle:
                    Show(MaxRound > 0);
                    break;
                case BattleManager.State.StartRound:
                    UpdateUI(BattleManager.Instance.CurrentRound);
                    break;
                case BattleManager.State.None:
                    Show(false);
                    break;
            }
        }

        [Button]
        public void Show(bool value) => container.SetActive(value);

        [Button]
        public void UpdateUI(int currentRound)
        {
            if (currentRound == 0)
                return;
            roundLabel.text = $"Round {currentRound}/{MaxRound}";
        }
    }
}
