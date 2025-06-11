using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class JourneyUIManager : Spawner<JourneyUIItem>
    {
        public static JourneyUIManager Instance { get; private set; }

        [SerializeField] GameObject container;
        [SerializeField] JourneyUIItem prefab;
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] HorizontalLayoutGroup titleLayoutGroup;
        [SerializeField] HorizontalLayoutGroup journeyLayoutGroup;
        [SerializeField] Slider slider;

        StageData stageData;

        void Awake() => Instance = this;

        private IEnumerator Start()
        {
            yield return null;
            BattleManager.Instance.CurrentState.RegisterListener(BattleManager_CurrentStateUpdate);
            SceneGameManager.Instance.OnCurrentJouneyUpdate += SetIndex;
        }

        private void OnDestroy()
        {
            BattleManager.Instance.CurrentState.UnregisterListener(BattleManager_CurrentStateUpdate);
            SceneGameManager.Instance.OnCurrentJouneyUpdate -= SetIndex;
        }

        private void BattleManager_CurrentStateUpdate(BattleManager.State state)
        {
            switch(state)
            {
                case BattleManager.State.InitiateBattle:
                    Show(false);
                    break;
                case BattleManager.State.None:
                    Show(true);
                    break;
            }
        }

        [Button]
        public void Show(bool value) => container.SetActive(value);

        [Button]
        public void Init(StageData stageData)
        {
            Clear();
            Show(true);

            this.stageData = stageData;
            for (int i = 0; i < stageData.journeys.Count; i++)
                Spawn(prefab, init: item => item.Init(stageData.journeys[i], i));
            slider.maxValue = stageData.journeys.Count - 1;
            slider.image.color = GameSettings_Battle.Instance.colorTheme.journeyFillColor;

            LayoutRebuilder.ForceRebuildLayoutImmediate(titleLayoutGroup.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(journeyLayoutGroup.transform as RectTransform);
        }

        [Button]
        public void SetIndex(int index)
        {
            index = Mathf.Clamp(index, 0, activeList.Count);
            label.text = $"{stageData.title} - {index + 1}/{activeList.Count}";

            spawnParent.transform.DOLocalMove(index * journeyLayoutGroup.spacing * Vector3.left, GameSettings_Battle.Instance.constantValues.journeyTransitionDuration);
            slider.DOValue(index, GameSettings_Battle.Instance.constantValues.journeyTransitionDuration / 4);

            for (var i = 0; i < activeList.Count; i++)
                activeList[i].SetState(i <= index ? JourneyUIItem.State.Active : JourneyUIItem.State.Inactive);

            LayoutRebuilder.ForceRebuildLayoutImmediate(titleLayoutGroup.transform as RectTransform);
        }

        public override void Clear()
        {
            activeList.Clear();
            base.Clear();
        }
    }
}
