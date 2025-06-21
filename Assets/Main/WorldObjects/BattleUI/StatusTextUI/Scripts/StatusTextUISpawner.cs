using System.Collections;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class StatusTextUISpawner : Spawner<StatusTextUI>
    {
        public static StatusTextUISpawner Instance { get; private set; }

        [SerializeField] StatusTextUI prefab;

        protected float RandomAdditionalDistance => AppSettings_Battle.Instance.constantValues.statusTextUISpawnerRandomAdditionalDistance;

        private void Awake() => Instance = this;
        public void Spawn(Vector3 worldPosition, Color colorLabel, float value, AppSettings_Battle.Type type = AppSettings_Battle.Type.None)
        {
            if(value != 0)
                Spawn(prefab, init: item => item.Init(worldPosition, RandomAdditionalDistance, colorLabel, value, type));
        }

        public void Spawn(Vector3 worldPosition, Color colorLabel, string text, AppSettings_Battle.Type type = AppSettings_Battle.Type.None)
        {
            if(!string.IsNullOrWhiteSpace(text))
                Spawn(prefab, init: item => item.Init(worldPosition, RandomAdditionalDistance, colorLabel, text, type));
        }
    }
}
