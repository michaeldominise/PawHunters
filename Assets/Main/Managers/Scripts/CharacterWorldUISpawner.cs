using UnityEngine;

namespace PawHunters
{
    public class CharacterWorldUISpawner : Spawner<EntityWorldUI>
    {
        public static CharacterWorldUISpawner Instance { get; private set; }

        [SerializeField] EntityWorldUI prefab;
        [SerializeField] Camera worldCamera;

        private void Awake() => Instance = this;
        private void Start() => PlayerSpawner.Instance.OnSpawned += Spawn;
        public void Spawn(EntityMainController entityMainController) => Spawn(prefab, init: item => item.Init(entityMainController, worldCamera));
    }
}
