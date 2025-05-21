using System;
using UnityEngine;

namespace PawHunters
{
    [Serializable]
    public class StateController<State> where State : Enum
    {
        [SerializeField] State value;
        public State Value
        {
            get => value;
            set
            {
                if (this.value.ToString() == value.ToString())
                    return;
                this.value = value;
                OnStateUpdate?.Invoke(this.value);
            }
        }

        public event Action<State> OnStateUpdate;
        public void OnStateUpdateClear() => OnStateUpdate = null;

        public StateController() { }
        public StateController(State value) => this.value = value;
    }
}
