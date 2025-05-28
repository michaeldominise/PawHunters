using UnityEngine;

namespace PawHunters
{
    public class EntityUIPortraitSpawner : Spawner<EntityUIPortrait>
    {
        public static EntityUIPortraitSpawner Instance { get; private set; }

        [SerializeField] EntityUIPortrait prefabPlayer;
        [SerializeField] EntityUIPortrait prefabEnemy;
        [SerializeField] int preSpawnCount = 3;

        private void Awake() => Instance = this;
        private void Start()
        {
            for(var x = 0; x < preSpawnCount; x++)
            {
                SpawnPlayer(null);
                SpawnEnemy(null);
            }

            Clear();
        }

        public EntityUIPortrait SpawnPlayer(EntityMainController entityMainController) => Spawn(entityMainController, true);
        public EntityUIPortrait SpawnEnemy(EntityMainController entityMainController) => Spawn(entityMainController, false);
        public EntityUIPortrait Spawn(EntityMainController entityMainController, bool isPlayer) => Spawn(isPlayer ? prefabPlayer : prefabEnemy, init: item => item.Init(entityMainController));
    }
}
