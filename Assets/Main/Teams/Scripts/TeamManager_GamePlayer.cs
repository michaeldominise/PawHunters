using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace PawHunters
{
    public class TeamManager_GamePlayer : TeamManager_Game
    {
        public static TeamManager_GamePlayer Instance { get; private set; }

        void Awake() => Instance = this;
        private void OnEnable() => EnvironmentManager.Instance.OnMove += OnMove;
        private void OnDisable() => EnvironmentManager.Instance.OnMove -= OnMove;
        private void OnMove() => spawnParent.transform.position = EnvironmentManager.Instance.GroundMiddleCenterPosition + offset;
        protected override void CurrentState_OnStateUpdate(EntityMainController entity)
        {
            if (entity.CurrentState.Value != EntityMainController.State.Dead)
                return;

            base.CurrentState_OnStateUpdate(entity);
            if (!IsAlive)
                EnvironmentManager.Instance.SetState(EnvironmentManager.State.Idle);
        }
    }
}