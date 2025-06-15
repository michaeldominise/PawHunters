using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class JourneyLogsUIManager : Spawner<JourneyLogItem>
    {
        public static JourneyLogsUIManager Instance { get; private set; }

        [SerializeField] JourneyLogItem prefab;
        [SerializeField] RectTransform layoutGroup;
        [SerializeField] ScrollRect scrollRect;

        float JourneyLogsTransitionDuration => GameSettings_Battle.Instance.constantValues.journeyLogsTransitionDuration;

        string[] battleDescriptions;
        string[] bossDescriptions;
        string[] positiveDescriptions;
        string[] negativeDescriptions;

        void Awake() => Instance = this;

        private IEnumerator Start()
        {
            yield return null;
            SceneGameManager.Instance.OnCurrentJouneyUpdate += OnCurrentJouneyUpdate;
        }

        public void Init()
        {
            battleDescriptions = SceneGameManager.Instance.StageData.data.battleDescriptions.Asset.text.Split('\n');
            bossDescriptions = SceneGameManager.Instance.StageData.data.bossDescriptions.Asset.text.Split('\n');
            positiveDescriptions = SceneGameManager.Instance.StageData.data.positiveDescriptions.Asset.text.Split('\n');
            negativeDescriptions = SceneGameManager.Instance.StageData.data.negativeDescriptions.Asset.text.Split('\n');
        }

        private void OnDestroy() => SceneGameManager.Instance.OnCurrentJouneyUpdate -= OnCurrentJouneyUpdate;
        void OnCurrentJouneyUpdate(int index) => Spawn(SceneGameManager.Instance.CurrentJourney, index);

        [Button]
        public void Spawn(JourneyData journeyData, int index) => Spawn(journeyData.JourneyType, journeyData.Title, GetDescription(journeyData), $"Day {index + 1}");

        [Button]
        public async void Spawn(JourneyData.Type journeyType, string title, string description, string day = "")
        {
            var item = Spawn(prefab, init: item => item.Init(journeyType, title, description, day));
            await Task.Delay(100);
            LayoutRebuilder.ForceRebuildLayoutImmediate(spawnParent as RectTransform);
            scrollRect.DOVerticalNormalizedPos(0, JourneyLogsTransitionDuration).SetEase(Ease.OutQuad);
        }

        public string GetDescription(JourneyData.Type journeyType)
            => journeyType switch
            {
                JourneyData.Type.Default => string.Empty,
                JourneyData.Type.Battle => battleDescriptions[Random.Range(0, battleDescriptions.Length)],
                JourneyData.Type.Boss => bossDescriptions[Random.Range(0, battleDescriptions.Length)],
                JourneyData.Type.Negative => negativeDescriptions[Random.Range(0, battleDescriptions.Length)],
                _ => positiveDescriptions[Random.Range(0, battleDescriptions.Length)],
            };

        public string GetDescription(JourneyData journeyData)
        {
            var description = GetDescription(journeyData.JourneyType);
            return $"{description}{(string.IsNullOrWhiteSpace(description) ? string.Empty : "\n")}{journeyData.AdditionalDescription}";
        }

        public override void Clear()
        {
            activeList.Clear();
            base.Clear();
        }
    }
}
