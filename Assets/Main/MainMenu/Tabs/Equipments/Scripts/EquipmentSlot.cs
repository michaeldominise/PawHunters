using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EquipmentSlot : EntityParent_Selection
    {
        [SerializeField] EquipmentType equipmentType;
        [SerializeField] StateObjects<State> stateObjects;
        [SerializeField] EquipmentPreviewItem equipmentPreviewItem;
        [SerializeField] AnimationUI animationUI;
        [SerializeField] SaveableEquipmentData data;

        public EquipmentPreviewItem EquipmentPreviewItem => equipmentPreviewItem;
        public EquipmentType EquipmentType => equipmentType;
        public SaveableEquipmentData Data => data;

        [Button]
        public override void SetState(State state)
        {
            base.SetState(state);
            stateObjects.SetActive(state);
        }

        public override Task Init(SaveableDataEntity data, int index, int layerSortingOrder, Action<EntityMainController> onCurrentState_OnStateUpdate)
        {
            var task = base.Init(data, index, layerSortingOrder, onCurrentState_OnStateUpdate);
            equipmentPreviewItem.gameObject.SetActive(data != null);
            if (data == null)
                animationUI.ScaleDown();
            else
                animationUI.ScaleNormal();
            return task;
        }
    }
}
