using System;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public abstract class JourneyData : ScriptableObject
    {
        public enum Type { Default, Battle, Boss, Positive, Negative, Reward }

        public abstract Type JourneyType { get; }
        public virtual string ButtonLabel => "Next";
        public virtual string Title { get; }
        public virtual string AdditionalDescription { get; }
        public virtual float TargetDistance => 2;
        public virtual Vector3 TargetPosition => Vector3.zero;

        public virtual void Init() => Execute();
        public virtual void Execute() => OnTransitionFinished();
        public virtual void OnTransitionFinished() => JourneyButtons.Instance.Init(response => End(), ButtonLabel);
        public virtual void End() => SceneGameManager.Instance.NextJourney();

        public virtual void PlayTransition(TeamManager_Game movingTeam, Vector3 targetPosition)
        {
            movingTeam.SetState(StateSpeed.State.Running);
            var movingTeamSpeed = movingTeam.MovementSpeed;
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
    }
}
