using System;
using UnityEngine;

namespace PawHunters
{
    public class StateController<State> : MonoBehaviour where State : Enum
    {
        [SerializeField] State currentState;
        public State CurrentState
        {
            get => currentState;
            protected set
            {
                if (currentState.ToString() == value.ToString())
                    return;
                currentState = value;
                OnStateUpdate?.Invoke(currentState);
            }
        }

        public event Action<State> OnStateUpdate;
    }
}
