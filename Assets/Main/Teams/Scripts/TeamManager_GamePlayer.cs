using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System;
using System.Collections;

namespace LabHavenInteractive.PawHunters
{
    public class TeamManager_GamePlayer : TeamManager_Game
    {
        public static TeamManager_GamePlayer Instance { get; private set; }

        void Awake() => Instance = this;

        public override void Init(SaveableTeamData teamData)
        {
            base.Init(teamData);
            SetState(StateSpeed.State.Walking);
        }

        public override void Move(Vector3 worldPosiion)
        {
            base.Move(worldPosiion);
            EnvironmentManager.Instance.Move(worldPosiion + offset);
        }

        protected override void CurrentState_OnStateUpdate(EntityMainController entity)
        {
            if (entity.IsAlive)
                return;

            base.CurrentState_OnStateUpdate(entity);
            if (!IsAlive)
                SetState(StateSpeed.State.Idle);
        }
    }
}