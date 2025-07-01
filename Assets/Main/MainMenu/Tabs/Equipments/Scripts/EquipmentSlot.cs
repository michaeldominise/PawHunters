using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EquipmentSlot : MonoBehaviour
    {
        public enum State { Empty, Occupied, Locked }

        [SerializeField] EquipmentType equipmentType;
        [SerializeField] StateObjects<State> stateObjects;
        [SerializeField] EquipmentPreviewItem equipmentPreviewItem;
        [SerializeField] AnimationUI animationUI;
        [SerializeField] SaveableEquipmentData data;

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        public EquipmentType EquipmentType => equipmentType;
        public SaveableEquipmentData Data => data;

        [Button]
        public void SetState(State state)
        {
            CurrentState.Value = state;
            stateObjects.SetActive(state);
        }

        public void Init(SaveableEquipmentData data)
        {
            this.data = data;
            if (data == null)
            {
                SetState(State.Empty);
                equipmentPreviewItem.Unload();
                animationUI.ScaleDown();
                return;
            }

            SetState(State.Occupied);
            equipmentPreviewItem.Init((int)equipmentType, data);
            animationUI.ScaleNormal();
        }
    }
}
