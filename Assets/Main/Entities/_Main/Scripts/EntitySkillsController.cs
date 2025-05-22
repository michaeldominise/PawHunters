using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace PawHunters
{
    public class EntitySkillsController : MonoBehaviour
    {
        public enum State { None, Attacking, AttackDone, Cooldown }

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        [SerializeField] Transform spawnPoint;

        public Transform SpawnPoint => spawnPoint;

        EntityMainController playerMainController;
        Coroutine atttackCoroutine;
        SaveableCharacterData CharacterData => playerMainController.CharacterData;

        public void Init(EntityMainController playerMainController)
        {
            CurrentState.Value = State.None;
            this.playerMainController = playerMainController;
            StartCoroutine(CheckEnemies());
        }

        IEnumerator CheckEnemies()
        {
            yield return new WaitForSeconds(Random.value * 3);
            while(true)
            {
                if (playerMainController.CurrentState.Value == EntityMainController.State.Dead)
                    break;

                if (CurrentState.Value == State.AttackDone)
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
            CurrentState.Value = State.Attacking;

            yield return null;
            //yield return new WaitForSeconds(CharacterData.inGameObjects.weaponData.attribute.attackDelay);

            //BulletSpawner.Instance.Spawn(playerMainController);

            CurrentState.Value = State.AttackDone;
            atttackCoroutine = null;
        }

        IEnumerator StartCooldown()
        {
            CurrentState.Value = State.Cooldown;
            yield return new WaitForSeconds(Random.Range(CharacterData.attribute.attackCooldown.x, CharacterData.attribute.attackCooldown.y));
            CurrentState.Value = State.None;
        }
    }
}
