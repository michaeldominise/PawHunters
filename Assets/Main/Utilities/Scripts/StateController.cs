using System;
using UnityEngine;

namespace LabHaven.PawHunters
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
                OnStateUpdateVoid?.Invoke();
            }
        }

        event Action<State> OnStateUpdate;
        event Action OnStateUpdateVoid;

        public void ClearListeners()
        {
            OnStateUpdateVoid = null;
            OnStateUpdate = null;
        }

        public StateController() { }
        public StateController(State value) => this.value = value;

        public StateController<State> RegisterListener(Action<State> onValueChange)
        {
            OnStateUpdate -= onValueChange;
            OnStateUpdate += onValueChange;
            return this;
        }

        public StateController<State> RegisterListener(Action onValueChange)
        {
            OnStateUpdateVoid -= onValueChange;
            OnStateUpdateVoid += onValueChange;
            return this;
        }

        public StateController<State> UnregisterListener(Action<State> onValueChange)
        {
            OnStateUpdate -= onValueChange;
            return this;
        }

        public StateController<State> UnregisterListener(Action onValueChange)
        {
            OnStateUpdateVoid -= onValueChange;
            return this;
        }
    }
}
