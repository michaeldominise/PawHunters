using UnityEngine;
using UnityEngine.InputSystem;

namespace LabHavenInteractive.PawHunters
{
    public class EntityMovementController_Joystick : EntityMovementController
    {
        [SerializeField] float speed = 10f;

        InputAction m_moveAction;
        Vector2 m_moveAmt;

        void Awake() => m_moveAction = InputSystem.actions.FindAction("Move");
        void Update() => m_moveAmt = m_moveAction.ReadValue<Vector2>();

        private void FixedUpdate()
        {
            switch (m_moveAction.phase)
            {
                case InputActionPhase.Started:
                    Move();
                    break;
                default:
                    CurrentState.Value = State.Idle;
                    break;
            }
        }

        void Move()
        {
            var xPos = Mathf.Clamp(transform.localPosition.x + m_moveAmt.x * Time.deltaTime * speed, EnvironmentManager.GroundBoundingBox.Left, EnvironmentManager.GroundBoundingBox.Right);
            var yPos = Mathf.Clamp(transform.localPosition.y + m_moveAmt.y * Time.deltaTime * speed, EnvironmentManager.GroundBoundingBox.Bottom, EnvironmentManager.GroundBoundingBox.Top);
            transform.position = new Vector3(xPos, yPos, -yPos);

            CurrentState.Value = State.Moving;
        }
    }
}
