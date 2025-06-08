using UnityEngine;

namespace PawHunters
{
    public abstract class JourneyData : ScriptableObject
    {
        public enum Type { Default, Battle, Boss }

        [TextArea]
        public string description;
        public string buttonLabel = "Next";
        public Type type;

        public virtual float TargetDistance => 2;

        public abstract void Init();
        public abstract void Execute();

        public virtual void PlayTransition(TeamManager_Game movingTeam, Vector3 targetPosition)
        {

            movingTeam.SetState(StateSpeed.State.Running);
            var movingTeamSpeed = movingTeam.Speed;
            movingTeam.OnMove += OnMove;

            void OnMove()
            {
                if (Mathf.Abs(targetPosition.x - movingTeam.SpawnParent.position.x) > TargetDistance - Mathf.Abs(movingTeamSpeed * Time.deltaTime))
                    return;

                movingTeam.OnMove -= OnMove;
                movingTeam.SetState(StateSpeed.State.Idle);
                movingTeam.Move(targetPosition - TargetDistance * Mathf.Sign(movingTeamSpeed) * Vector3.right);
                OnTransitionFinished();
            }
        }

        public virtual void OnTransitionFinished() => SceneGameManager.Instance.NextJourney();
    }
}
