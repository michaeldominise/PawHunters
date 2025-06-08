using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System;
using UnityEngine.Serialization;

namespace PawHunters
{
    public class TeamManager_Game : TeamManager
    {
        public Vector3 offset;
        [ShowInInspector, ReadOnly] public StateController<StateSpeed.State> CurrentState { get; private set; } = new();

        public event Action OnMove;

        public virtual float Speed => GameSettings_Battle.Instance.constantValues.GetSpeed(CurrentState.Value);

        public override void Init(SaveableTeamData teamData)
        {
            base.Init(teamData);
            ExecuteSetupSkills();
        }

        [Button]
        public virtual void SetState(StateSpeed.State state) => CurrentState.Value = state;
        void Update()
        { 
            if (CurrentState.Value == StateSpeed.State.Idle)
                return;
            Move(spawnParent.transform.position + Speed * Time.deltaTime * Vector3.right);
        }

        public virtual void Move(Vector3 worldPosition)
        {
            spawnParent.transform.position = worldPosition;
            OnMove?.Invoke();
        }

        public void ExecuteSetupSkills()
        {
            foreach (var entity in AliveEntityList)
                _ = entity.EntitySkillsController.Execute(GameActionTriggersManager.TriggerType.SetupPhase);
        }

        [Button]
        public void Kill() => AliveEntityList.ForEach(x => x.EntityHealthController.Kill());
    }
}
