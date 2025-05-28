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

        public void UpdateUI(int currentRound)
        {
            container.SetActive(currentRound > 0 && MaxRound > 0);
            if (currentRound == 0)
                return;
            roundLabel.text = $"Round {currentRound}/{MaxRound}";
        }
    }
}
