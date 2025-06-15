using System;
using System.Collections;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class SceneGameManager : SingletonMonoBehaviour<SceneGameManager>
    {
        [SerializeField] protected GameSettings_Battle gameSettings_Battle;

        [ShowInInspector, ReadOnly] int CurrentJourneyIndex { get; set; } = -1;
        [ShowInInspector, ReadOnly] public bool IsLastJourney => StageData?.data.journeys.Count == CurrentJourneyIndex + 1;
        public event Action<int> OnCurrentJouneyUpdate;

        public SaveableTeamData PlayerTeamData => GameManager.Instance?.appValues.playerTeamData;
        public StageData StageData => GameManager.Instance?.appValues.stageData.Asset;
        public JourneyData CurrentJourney => StageData.data.journeys[CurrentJourneyIndex].Asset;
        public GameSettings_Battle GameSettings_Battle => gameSettings_Battle;

        IEnumerator Start()
        {
            yield return null;
            yield return null;
            _ = Init();
        }

        protected async virtual Task Init()
        {
            await GameManager.Instance.appValues.stageData.Load();
            await StageData.data.LoadAssets(StageData.AssetType.all);
            await PlayerTeamData.LoadAssets();
            EnvironmentManager.Instance.Init(StageData.data.environmentItem.Asset);
            JourneyUIManager.Instance.Init(StageData);
            TeamManager_GamePlayer.Instance.Init(PlayerTeamData);
            InitialUI.Instance.Init();
            JourneyLogsUIManager.Instance.Init();
            NextJourney();
        }

        public virtual void EndJourney() => StageData.data.journeys[CurrentJourneyIndex].Asset.End();

        [Button]
        public virtual void NextJourney()
        {
            if (TeamManager_GameEnemy.Instance.IsAlive)
                TeamManager_GameEnemy.Instance.Kill();
            CurrentJourneyIndex++;

            if (StageData.data.journeys.Count == CurrentJourneyIndex)
                JourneyComplete();
            else
            {
                TeamManager_GamePlayer.Instance.SetState(StateSpeed.State.Walking);
                StageData.data.journeys[CurrentJourneyIndex].Asset.Execute();
                OnCurrentJouneyUpdate?.Invoke(CurrentJourneyIndex);
            }
        }

        [Button]
        public virtual void JourneyFailed() => DefeatUI.Instance.Show(true);

        [Button]
        public virtual void JourneyComplete() => VictoryUI.Instance.Show(true);

        protected virtual void OnDestroy()
        {
            if (StageData == null)
                return;
            StageData.data.UnloadAssets(StageData.AssetType.all);
            GameManager.Instance.appValues.stageData.Unload();
        }
    }
}
