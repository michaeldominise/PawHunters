using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace PawHunters
{
    public class EntitySkillsController : MonoBehaviour
    {
        public enum State { None, Attacking, AttackDone, Cooldown }

        [SerializeField] Transform spawnPoint;
        [SerializeField] State currentState;
        public State CurrentState
        {
            get => currentState;
            set
            {
                if (currentState == value)
                    return;
                currentState = value;
                OnStateUpdate?.Invoke(currentState);
            }
        }

        public event Action<State> OnStateUpdate;

        public Transform SpawnPoint => spawnPoint;

        EntityMainController playerMainController;
        Coroutine atttackCoroutine;
        CharacterData CharacterData => playerMainController.CharacterData;

        public void Init(EntityMainController playerMainController)
        {
            currentState = State.None;
            this.playerMainController = playerMainController;
            StartCoroutine(CheckEnemies());
        }

        IEnumerator CheckEnemies()
        {
            yield return new WaitForSeconds(Random.value * 3);
            while(true)
            {
                if (playerMainController.CurrentState == EntityMainController.State.Dead)
                    break;

                if (currentState == State.AttackDone)
                    yield return StartCooldown();
                RefreshTarget();
                yield return null;
            }
        }

        void RefreshTarget()
        {
            
        }

        void StartAttack()
        {
            if(atttackCoroutine == null)
                atttackCoroutine = StartCoroutine(_StartAttack());
        }

        IEnumerator _StartAttack()
        {
            CurrentState = State.Attacking;

            yield return null;
            yield return new WaitForSeconds(CharacterData.inGameObjects.weaponData.attribute.attackDelay);

            BulletSpawner.Instance.Spawn(playerMainController);

            CurrentState = State.AttackDone;
            atttackCoroutine = null;
        }

        IEnumerator StartCooldown()
        {
            CurrentState = State.Cooldown;
            yield return new WaitForSeconds(Random.Range(CharacterData.attribute.attackCooldown.x, CharacterData.attribute.attackCooldown.y));
            CurrentState = State.None;
        }
    }
}
