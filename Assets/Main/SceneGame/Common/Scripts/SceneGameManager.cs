using System;
using System.Collections;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class SceneGameManager : SingletonMonoBehaviour<SceneGameManager>
    {
        [SerializeField] protected GameSettings_Battle gameSettings_Battle;
        [SerializeField] protected StageData stageData;
        [SerializeField] protected SaveableTeamData teamData;

        [ShowInInspector, ReadOnly] int CurrentJourneyIndex { get; set; } = -1;
        public event Action<int> OnCurrentJouneyUpdate;

        public StageData StageData => stageData;
        public GameSettings_Battle GameSettings_Battle => gameSettings_Battle;

        IEnumerator Start()
        {
            yield return null;
            yield return null;
            Init();
        }

        protected virtual void Init()
        {
            EnvironmentManager.Instance.Init(stageData.environmentItem);
            JourneyUIManager.Instance.Init(stageData);
            TeamManager_GamePlayer.Instance.Init(teamData);
            EnvironmentManager.Instance.SetState(EnvironmentManager.State.Walking);
            InitialUI.Instance.Init();
            NextJourney(true);
        }

        [Button]
        public async virtual void NextJourney(bool isInstant = false)
        {
            if(!isInstant)
                await Task.Delay(500);

            if (TeamManager_GameEnemy.Instance.IsAlive)
                TeamManager_GameEnemy.Instance.Kill();
            CurrentJourneyIndex++;

            if (stageData.journeys.Count == CurrentJourneyIndex)
                JourneyComplete();
            else
            {
                EnvironmentManager.Instance.SetState(EnvironmentManager.State.Walking);
                stageData.journeys[CurrentJourneyIndex].Init();
                OnCurrentJouneyUpdate?.Invoke(CurrentJourneyIndex);
            }
        }

        [Button]
        public virtual void JourneyFailed() => DefeatUI.Instance.Show(true);

        [Button]
        public virtual void JourneyComplete() => VictoryUI.Instance.Show(true);
    }
}
