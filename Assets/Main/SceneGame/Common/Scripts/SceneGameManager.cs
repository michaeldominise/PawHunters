using System;
using System.Collections;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class SceneGameManager : MonoBehaviour
    {
        public static SceneGameManager Instance { get; private set; }

        [SerializeField] protected GameSettings_Battle gameSettings_Battle;
        [SerializeField] protected LevelData levelData;
        [SerializeField] protected SaveableTeamData teamData;

        [ShowInInspector, ReadOnly] int CurrentJourneyIndex { get; set; } = -1;
        public event Action<int> OnCurrentJouneyUpdate;

        public LevelData LevelData => levelData;
        public GameSettings_Battle GameSettings_Battle => gameSettings_Battle;

        private void Awake() => Instance = this;
        IEnumerator Start()
        {
            yield return null;
            yield return null;
            Init();
        }

        protected virtual void Init()
        {
            EnvironmentManager.Instance.Init(levelData.environmentItem);
            TeamManager_GamePlayer.Instance.Init(teamData);
            JourneyUIManager.Instance.Init(levelData);
            NextJourney();
        }

        [Button]
        public async virtual void NextJourney()
        {
            await Task.Delay(500);
            if (TeamManager_GameEnemy.Instance.IsAlive)
                TeamManager_GameEnemy.Instance.Kill();
            EnvironmentManager.Instance.SetState(EnvironmentManager.State.Walking);
            CurrentJourneyIndex++;

            if (levelData.journeys.Count == CurrentJourneyIndex)
                JourneyComplete();
            else
            {
                levelData.journeys[CurrentJourneyIndex].Init();
                OnCurrentJouneyUpdate?.Invoke(CurrentJourneyIndex);
            }
        }

        [Button]
        public virtual void JourneyFailed()
        {
        }

        [Button]
        public virtual void JourneyComplete()
        {
        }
    }
}
