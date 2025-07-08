using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EquipmentSlotManager_Battle : EquipmentSlotManager
    {
        [SerializeField] GameObject container;

        List<EntityMainController> AliveEntityList => equipmentSlots.Select(x => x.EquipmentPreviewItem.Asset).ToList();

        private IEnumerator Start()
        {
            yield return null;
            BattleManager.Instance.CurrentState.RegisterListener(BattleManager_OnStateValueChange);
        }

        private void BattleManager_OnStateValueChange(BattleManager.State state) => container.SetActive(state != BattleManager.State.None);

        public override async Task Init(SaveableTeamData teamData)
        {
            this.teamData = teamData;
            var equipments = teamData.Equipments.Where(x => x != null && x.InstanceData.instanceId != -1).ToList();
            var tasks = new List<Task>();
            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                if(i >= equipments.Count)
                {
                    equipmentSlots[i].gameObject.SetActive(false);
                    continue;
                }
                
                SaveableEquipmentData equipmentData = equipments[i];
                equipmentSlots[i].gameObject.SetActive(true);
                tasks.Add(equipmentSlots[i].Init(equipmentData));
            }

            await Task.WhenAll(tasks);
        }

        public async Task ExecuteSkills(GameActionTriggersManager.TriggerType trigger, object srouceTrigger = null, Func<bool> condition = null)
        {
            if (!(condition?.Invoke() ?? true))
                return;
            foreach (var entity in AliveEntityList)
            {
                await entity.EntitySkillsController.Execute(trigger, srouceTrigger);
                if (!(condition?.Invoke() ?? true))
                    return;
            }
        }
    }
}
