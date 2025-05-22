using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace PawHunters
{
    public class TeamManager_GameEnemy : TeamManager_Game
    {
        public static TeamManager_GameEnemy Instance { get; private set; }

        void Awake() => Instance = this;

        [Button]
        public void SpawnEnemy(SaveableTeamData teamData)
        {
            spawnParent.transform.position = EnvironmentManager.Instance.GroundMiddleCenterPosition + offset;
            EnvironmentManager.Instance.SetState(EnvironmentManager.State.Running);
            Init(teamData);
        }

        protected override void CurrentState_OnStateUpdate(EntityMainController entity)
        {
            if (entity.CurrentState.Value != EntityMainController.State.Dead)
                return;

            base.CurrentState_OnStateUpdate(entity);
            if (!IsAlive)
                SceneGameManager_Hunt.Instance.NextJourney();
        }
    }
}
