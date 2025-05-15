using UnityEngine;
using UnityEngine.Events;

namespace PawHunters
{
    public class AnimatorStateHandler<Data> : StateMachineBehaviour
    {
        public UnityEvent StateEnterEvent;
        public UnityEvent StateUpdateEvent;
        public UnityEvent StateExit;

        protected Data data;

        public virtual void Init(Data data) => this.data = data;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => StateEnterEvent?.Invoke();
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => StateUpdateEvent?.Invoke();
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => StateExit?.Invoke();
    }
}