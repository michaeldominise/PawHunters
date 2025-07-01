using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EquipmentSlotManager : MonoBehaviour
    {
        [SerializeField] List<EquipmentSlot> equipmentSlots;
        [SerializeField] SaveableTeamData_CharacterInstance teamData;

        public void Init(SaveableTeamData_CharacterInstance teamData)
        {
            this.teamData = teamData;
            for (int i = 0; i < teamData.Equipments.Count; i++)
            {
                SaveableEquipmentData equipmentData = teamData.Equipments[i];
                equipmentSlots[i].Init(equipmentData);
            }
        }

        public bool Equip(SaveableEquipmentData equipmentData)
        {
            var equipmentPrefab = equipmentData.GetPrefab() as EquipmentMainController;
            var equipmentIndex = equipmentSlots.FindIndex(x => x.EquipmentType == equipmentPrefab.EquipmentType);
            if (equipmentIndex < 0)
                return false;

            var equipmentSlot = equipmentSlots[equipmentIndex];
            if (equipmentSlot.CurrentState.Value == EquipmentSlot.State.Locked)
                return false;

            var value = true;
            if (equipmentSlot.CurrentState.Value != EquipmentSlot.State.Empty && equipmentSlot.Data == equipmentData)
            {
                equipmentData = null;
                value = false;
            }

            equipmentSlot.Init(equipmentData);
            teamData.equipmentInstanceList[equipmentIndex].instanceId = equipmentData == null ? -1 : equipmentData.InstanceData.instanceId;
            teamData.SetDirty();
            return value;
        }
    }
}
