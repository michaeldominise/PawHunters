using System.Collections;
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

        private void Awake() => Instance = this;
        protected virtual IEnumerator Start()
        {
            yield return null;
            EnvironmentManager.Instance.Init(levelData.environmentItem);
            TeamManager_GamePlayer.Instance.Init(teamData);
            NextJourney();
        }

        [Button]
        public virtual void NextJourney()
        {
            if (TeamManager_GameEnemy.Instance.IsAlive)
                TeamManager_GameEnemy.Instance.Kill();
            EnvironmentManager.Instance.SetState(EnvironmentManager.State.Walking);
            CurrentJourney++;
            levelData.journeys[CurrentJourney].Init();
        }
    }
}
