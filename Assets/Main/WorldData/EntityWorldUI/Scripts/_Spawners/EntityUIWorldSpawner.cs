using UnityEngine;

namespace PawHunters
{
    public class EntityUIWorldSpawner : Spawner<EntityUIWorld>
    {
        public static EntityUIWorldSpawner Instance { get; private set; }

        [SerializeField] EntityUIWorld prefab;

        private void OnEnable()
        {
            TeamManager_GamePlayer.Instance.OnSpawned += Spawn;
            TeamManager_GameEnemy.Instance.OnSpawned += Spawn;
        }

        private void OnDisable()
        {
            TeamManager_GamePlayer.Instance.OnSpawned -= Spawn;
            TeamManager_GameEnemy.Instance.OnSpawned -= Spawn;
        }

        private void Awake() => Instance = this;
        public void Spawn(EntityMainController entityMainController) => Spawn(prefab, init: item => item.Init(entityMainController));
    }
}
