using UnityEngine;

namespace LabHaven.PawHunters
{
    public class EntityAnimationControllerSetState : AnimatorStateHandler<EntityAnimationController>
    {
        [SerializeField] EntityAnimationController.State state;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            data.OnAnimationStateEnter(state);
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            data.OnAnimationStateUpdate(state);
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            data.OnAnimationStateExit(state);
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}
