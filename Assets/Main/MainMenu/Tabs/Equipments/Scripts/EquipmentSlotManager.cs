using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EquipmentSlotManager : MonoBehaviour
    {
        [SerializeField] protected List<EquipmentSlot> equipmentSlots;
        [SerializeField] protected SaveableTeamData teamData;

        SaveableTeamData_CharacterInstance TeamData_CharacterInstance => teamData as SaveableTeamData_CharacterInstance;

        public virtual async Task Init(SaveableTeamData teamData)
        {
            this.teamData = teamData;
            var tasks = new List<Task>();
            for (int i = 0; i < teamData.Equipments.Count; i++)
            {
                SaveableEquipmentData equipmentData = teamData.Equipments[i];
                tasks.Add(equipmentSlots[i].Init(equipmentData));
            }
            await Task.WhenAll(tasks);
        }

        public bool Equip(SaveableEquipmentData equipmentData)
        {
            var equipmentPrefab = equipmentData.GetPrefab() as EquipmentMainController;
            var equipmentIndex = equipmentSlots.FindIndex(x => x.EquipmentType == equipmentPrefab.EquipmentType);
            if (equipmentIndex < 0)
                return false;

            return Equip(equipmentData, equipmentIndex, false);
        }

        public bool Equip(SaveableEquipmentData equipmentData, int equipmentIndex, bool forceEquip = true)
        {
            var equipmentSlot = equipmentSlots[equipmentIndex];
            if (equipmentSlot.CurrentState.Value == EquipmentSlot.State.Locked)
                return false;

            var value = true;
            if (!forceEquip && equipmentSlot.CurrentState.Value != EquipmentSlot.State.Empty && equipmentSlot.Data == equipmentData)
            {
                equipmentData = null;
                value = false;
            }

            _ = equipmentSlot.Init(equipmentData);
            TeamData_CharacterInstance.equipmentInstanceList[equipmentIndex].instanceId = equipmentData == null ? -1 : equipmentData.InstanceData.instanceId;
            TeamData_CharacterInstance.SetDirty();
            return value;
        }
    }
}
