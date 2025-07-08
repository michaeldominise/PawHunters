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
        public void Spawn(Vector3 worldPosition, Color colorLabel, float value, Sprite sprite = null)
        {
            if(value != 0)
                Spawn(prefab, init: item => item.Init(worldPosition, RandomAdditionalDistance, colorLabel, value, sprite));
        }

        public void Spawn(Vector3 worldPosition, Color colorLabel, string text, Sprite sprite = null)
        {
            if(!string.IsNullOrWhiteSpace(text))
                Spawn(prefab, init: item => item.Init(worldPosition, RandomAdditionalDistance, colorLabel, text, sprite));
        }
    }
}
