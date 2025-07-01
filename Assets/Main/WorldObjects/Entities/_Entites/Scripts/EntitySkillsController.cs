using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
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
            InitSkills();
        }

        void InitSkills()
        {
            skillList.Clear();
            if(entityMainController.Data != null)
                skillList.AddRange(entityMainController.Data.SkillDataList);
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

        public void AddSkill(SkillData skillData, bool executeIfSetupPhase = true)
        {
            var index = skillList.FindIndex(x => !string.IsNullOrWhiteSpace(x.familyName) && x.familyName == skillData.familyName);
            if (executeIfSetupPhase && skillData.trigger.HasFlag(GameActionTriggersManager.TriggerType.SetupPhase))
                _ = skillData.Execute(GameActionTriggersManager.TriggerType.SetupPhase, entityMainController);
            else if (index < 0)
                skillList.Add(skillData);
            else
                skillList[index] = skillData;
        }
    }
}
