using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class RoundUIManager : SingletonMonoBehaviour<RoundUIManager>
    {
        [SerializeField] GameObject container;
        [SerializeField] TextMeshProUGUI roundLabel;

        int MaxRound => SceneGameManager.Instance.StageData.maxRound;

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
                case BattleManager.State.InitiateBattle:
                    Show(MaxRound > 0);
                    break;
                case BattleManager.State.BeginRound:
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
