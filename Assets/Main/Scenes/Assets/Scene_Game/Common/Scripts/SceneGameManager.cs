using System;
using System.Collections;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public abstract class SceneGameManager : SingletonMonoBehaviour<SceneGameManager>
    {
        [SerializeField] protected AppSettings_Battle appSettings_Battle;

        [ShowInInspector, ReadOnly] int CurrentJourneyIndex { get; set; } = -1;
        [ShowInInspector, ReadOnly] public bool IsLastJourney => StageData?.data.journeys.Count == CurrentJourneyIndex + 1;
        public event Action<int> OnCurrentJouneyUpdate;

        protected abstract AssetReferenceMasterID<StageData> StageDataAssetReference { get; }
        public StageData StageData => StageDataAssetReference.Asset;
        public SaveableTeamData PlayerTeamData => AppManager.Instance?.userData.playerTeamData;
        public JourneyData CurrentJourney => StageData.data.journeys[CurrentJourneyIndex].Asset;
        public AppSettings_Battle AppSettings_Battle => appSettings_Battle;

        IEnumerator Start()
        {
            yield return null;
            yield return null;
            _ = Init();
        }

        protected async virtual Task Init()
        {
            await StageDataAssetReference.Load();
            await StageData.data.LoadAssets(StageData.AssetType.all);
            await PlayerTeamData.LoadAssets(SaveableCharacterData.AssetType.all);
            EnvironmentManager.Instance.Init(StageData.data.environmentItem.Asset);
            JourneyUIManager.Instance.Init(StageData);
            TeamManager_GamePlayer.Instance.Init(PlayerTeamData);
            InitialUI.Instance.Init(StageData.title);
            JourneyLogsUIManager.Instance.Init();
            SceneTransitionLoader.Instance.Hide(AppSettings_Battle.Instance.constantValues.overlayFadeOutDelayDuration);
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
            PlayerTeamData.UnloadAssets(SaveableCharacterData.AssetType.all);
            AppManager.Instance.userData.battleData.huntStageData.Unload();
        }
    }
}
