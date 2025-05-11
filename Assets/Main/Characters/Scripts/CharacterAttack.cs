using Assets.FantasyMonsters.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PawHunters
{
    [RequireComponent(typeof(Monster))]
    public class CharacterAtack : MonoBehaviour
    {
        [SerializeField] Monster monster;

        InputAction m_moveAttack;

        void Awake()
        {
            m_moveAttack = InputSystem.actions.FindAction("Attack");
        }

        private void Update()
        {
            if (m_moveAttack.WasPressedThisFrame())
                Attack();
        }

        void Attack()
        {
            monster.Attack();
        }
    }
}
