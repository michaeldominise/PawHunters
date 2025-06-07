using System.Collections;
using Sirenix.OdinInspector;
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

        private IEnumerator Start()
        {
            yield return null;
            BattleManager.Instance.CurrentState.RegisterListener(BattleManager_CurrentStateUpdate);
            Init();
        }

        private void OnDestroy() => BattleManager.Instance.CurrentState.UnregisterListener(BattleManager_CurrentStateUpdate);

        private void BattleManager_CurrentStateUpdate(BattleManager.State state)
        {
            switch (state)
            {
                case BattleManager.State.InitiateBattle:
                    Show(true);
                    break;
                case BattleManager.State.None:
                    Clear();
                    Show(false);
                    break;
            }
        }

        private void Init()
        {
            for (var x = 0; x < preSpawnCount; x++)
            {
                SpawnPlayer(null);
                SpawnEnemy(null);
            }

            Clear();
        }

        [Button]
        public void Show(bool value) => spawnParent.gameObject.SetActive(value);
        public EntityUIPortrait SpawnPlayer(EntityMainController entityMainController) => Spawn(entityMainController, true);
        public EntityUIPortrait SpawnEnemy(EntityMainController entityMainController) => Spawn(entityMainController, false);
        public EntityUIPortrait Spawn(EntityMainController entityMainController, bool isPlayer) => Spawn(isPlayer ? prefabPlayer : prefabEnemy, init: item => item.Init(entityMainController));
    }
}
