using System.Collections;
using UnityEngine;

namespace PawHunters
{
    public class StatusTextUISpawner : Spawner<StatusTextUI>
    {
        public static StatusTextUISpawner Instance { get; private set; }

        [SerializeField] StatusTextUI prefab;

        protected float RandomAdditionalDistance => GameSettings_Battle.Instance.constantValues.statusTextUISpawnerRandomAdditionalDistance;

        private void Awake() => Instance = this;
        public void Spawn(Vector3 worldPosition, Color colorLabel, float value, GameSettings_Battle.Type type = GameSettings_Battle.Type.None)
        {
            if(value != 0)
                Spawn(prefab, init: item => item.Init(worldPosition, RandomAdditionalDistance, colorLabel, value, type));
        }

        public void Spawn(Vector3 worldPosition, Color colorLabel, string text, GameSettings_Battle.Type type = GameSettings_Battle.Type.None)
        {
            if(!string.IsNullOrWhiteSpace(text))
                Spawn(prefab, init: item => item.Init(worldPosition, RandomAdditionalDistance, colorLabel, text, type));
        }
    }
}
