using System.Collections;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EntityUIWorldSpawner : Spawner<EntityUIWorld>
    {
        public static EntityUIWorldSpawner Instance { get; private set; }

        [SerializeField] EntityUIWorld prefab;

        private IEnumerator Start()
        {
            yield return null;
            TeamManager_GamePlayer.Instance.OnSpawned += Spawn;
            TeamManager_GameEnemy.Instance.OnSpawned += Spawn;
        }

        private void OnDestroy()
        {
            TeamManager_GamePlayer.Instance.OnSpawned -= Spawn;
            TeamManager_GameEnemy.Instance.OnSpawned -= Spawn;
        }

        private void Awake() => Instance = this;
        public void Spawn(EntityMainController entityMainController) => Spawn(prefab, init: item => item.Init(entityMainController));
    }
}
