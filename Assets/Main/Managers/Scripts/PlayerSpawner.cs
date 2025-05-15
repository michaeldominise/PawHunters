using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace PawHunters
{
    public class PlayerSpawner : Spawner<EntityMainController>
    {
        public static PlayerSpawner Instance { get; private set; }

        [SerializeField] int spawnRange = 10;
        [SerializeField] List<CharacterData> characterList;

        public List<EntityMainController> Spawnedlist => spawnedlist;

        private void Awake() => Instance = this;

        public void StartGame(int spawnCount)
        { 
            for (var x = 0; x < spawnCount; x++)
                Spawn(characterList[Random.Range(0, characterList.Count)]);
        }

        public void Spawn(CharacterData characterData)
            => Spawn(characterData.inGameObjects.playerPrefab, x => x.CharacterData == characterData,
                init: item => item.Init(characterData));

        protected override Vector3 GetSpawnPoint()
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRange;
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                    return hit.position;
            }
            return Vector3.zero;
        }
    }
}
