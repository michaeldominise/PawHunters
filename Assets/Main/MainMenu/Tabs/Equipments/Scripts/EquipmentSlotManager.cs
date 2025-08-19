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
        int layerSortingOrder;
        Action<EntityMainController> onCurrentState_OnStateUpdate;

        public virtual async Task Init(SaveableTeamData teamData, int layerSortingOrder, Action<EntityMainController> onCurrentState_OnStateUpdate)
        {
            this.teamData = teamData;
            this.layerSortingOrder = layerSortingOrder;
            this.onCurrentState_OnStateUpdate = onCurrentState_OnStateUpdate;
            var tasks = new List<Task>();
            for (int i = 0; i < teamData.Equipments.Count; i++)
                tasks.Add(EntityLoad(i));
            await Task.WhenAll(tasks);
        }

        public virtual async Task EntityLoad(int index) => await equipmentSlots[index].Init(teamData.Equipments[index], index, layerSortingOrder, onCurrentState_OnStateUpdate);

        public virtual async Task Equip(SaveableEquipmentData equipmentData, EquipmentType equipmentType)
        {
            var equipmentIndex = equipmentSlots.FindIndex(x => x.EquipmentType == equipmentType);
            if (equipmentIndex < 0)
                return;

            var equipmentSlot = equipmentSlots[equipmentIndex];
            if (equipmentSlot.CurrentState.Value == EntityParent_Selection.State.Locked)
                return;

            TeamData_CharacterInstance.equipmentInstanceList[equipmentIndex].instanceId = equipmentData == null ? -1 : equipmentData.InstanceData.instanceId;
            await EntityLoad(equipmentIndex);
            TeamData_CharacterInstance.SetDirty();
        }
    }
}
