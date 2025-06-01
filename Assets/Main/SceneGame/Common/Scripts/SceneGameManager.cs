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

        [SerializeField] LevelData levelData;
        [SerializeField] SaveableTeamData teamData;
        [ShowInInspector, ReadOnly] public int CurrentJourney { get; private set; } = -1;

        public LevelData LevelData => levelData;

        private void Awake() => Instance = this;
        protected virtual IEnumerator Start()
        {
            yield return null;
            yield return null;
            EnvironmentManager.Instance.Init(levelData.environmentItem);
            TeamManager_GamePlayer.Instance.Init(teamData);
            NextJourney();
        }

        [Button]
        public async virtual void NextJourney()
        {
            await Task.Delay(500);
            if (TeamManager_GameEnemy.Instance.IsAlive)
                TeamManager_GameEnemy.Instance.Kill();
            EnvironmentManager.Instance.SetState(EnvironmentManager.State.Walking);
            CurrentJourney++;
            if (levelData.journeys.Count == CurrentJourney)
                JourneyComplete();
            else
                levelData.journeys[CurrentJourney].Init();
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
