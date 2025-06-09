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
        List<JourneyLogItem> itemList = new();

        string[] battleDescriptions;
        string[] bossDescriptions;

        void Awake() => Instance = this;

        private IEnumerator Start()
        {
            yield return null;
            SceneGameManager.Instance.OnCurrentJouneyUpdate += OnCurrentJouneyUpdate;
            Init();
        }

        void Init()
        {
            battleDescriptions = SceneGameManager.Instance.StageData.battleDescriptions.text.Split('\n');
            bossDescriptions = SceneGameManager.Instance.StageData.bossDescriptions.text.Split('\n');
        }

        private void OnDestroy() => SceneGameManager.Instance.OnCurrentJouneyUpdate -= OnCurrentJouneyUpdate;
        void OnCurrentJouneyUpdate(int index) => Spawn(SceneGameManager.Instance.CurrentJourney, index);

        [Button]
        public void Spawn(JourneyData journeyData, int index) => Spawn(journeyData.type, journeyData.title, GetDescription(journeyData), $"Day {index + 1}");

        [Button]
        public async void Spawn(JourneyData.Type journeyType, string title, string description, string day = "")
        {
            var item = Spawn(prefab, init: item => item.Init(journeyType, title, description, day));
            itemList.Add(item);
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.transform as RectTransform);
            await Task.Yield();
            scrollRect.DOVerticalNormalizedPos(0, JourneyLogsTransitionDuration).SetEase(Ease.OutQuad);
        }

        public string GetDescription(JourneyData journeyData)
        {
            if (!string.IsNullOrWhiteSpace(journeyData.overrideDescription))
                return journeyData.overrideDescription;
            else
                return journeyData.type switch
                {
                    JourneyData.Type.Default => journeyData.overrideDescription,
                    JourneyData.Type.Battle => battleDescriptions[Random.Range(0, battleDescriptions.Length)],
                    JourneyData.Type.Boss => bossDescriptions[Random.Range(0, battleDescriptions.Length)],
                    _ => journeyData.overrideDescription,
                };
        }

        public override void Clear()
        {
            itemList.Clear();
            base.Clear();
        }
    }
}
