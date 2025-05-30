using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace PawHunters
{
    public class EntitySkillsController : MonoBehaviour
    {
        public enum State { None, Attacking, AttackDone }

        [SerializeField] List<SkillData> skillList;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

        EntityMainController entityMainController;

        public void Init(EntityMainController entityMainController)
        {
            CurrentState.Value = State.None;
            this.entityMainController = entityMainController;
        }

        public async Task Execute(GameActionTriggersManager.TriggerType trigger, object srouceTrigger = null)
        {
            if (!entityMainController.IsAlive)
                return;

            CurrentState.Value = State.Attacking;

            foreach (var skill in skillList)
            {
                await skill.Execute(trigger, entityMainController, srouceTrigger);
                if (!entityMainController.IsAlive)
                    return;
            }

            CurrentState.Value = State.AttackDone;
        }
    }
}
