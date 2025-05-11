using Assets.FantasyMonsters.Common.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PawHunters
{
    [RequireComponent(typeof(Monster))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] Monster monster;
        [SerializeField] float speed = 10f;

        InputAction m_moveAction;
        Vector2 m_moveAmt;

        void Awake()
        {
            m_moveAction = InputSystem.actions.FindAction("Move");
        }

        private void Update()
        {
            m_moveAmt = m_moveAction.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            if (!m_moveAction.IsPressed())
            {
                monster.SetState(MonsterState.Idle);
                return;
            }

            monster.SetState(MonsterState.Walk);
            var xPos = Mathf.Clamp(transform.localPosition.x + m_moveAmt.x * Time.deltaTime * speed, EnvironmentManager.GroundBoundingBox.Left, EnvironmentManager.GroundBoundingBox.Right);
            var yPos = Mathf.Clamp(transform.localPosition.y + m_moveAmt.y * Time.deltaTime * speed, EnvironmentManager.GroundBoundingBox.Bottom, EnvironmentManager.GroundBoundingBox.Top);
            transform.position = new Vector3(xPos, yPos, -yPos);

            if (m_moveAmt.x > 0)
                EnvironmentManager.Instance.MoveCameraForward();
        }
    }
}
