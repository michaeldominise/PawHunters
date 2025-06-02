using System.Collections;
using UnityEngine;

namespace PawHunters
{
    public class StatusTextUISpawner : Spawner<StatusTextUI>
    {
        public static StatusTextUISpawner Instance { get; private set; }

        [SerializeField] CommonIconSettings commonIconSettings;
        [SerializeField] StatusTextUI prefab;

        protected float RandomAdditionalDistance => GlobalSettings.Instance.gameSettings.statusTextUISpawnerRandomAdditionalDistance;

        private void Awake() => Instance = this;
        public void Spawn(Vector3 worldPosition, Color colorLabel, float value, CommonIconSettings.Icon icon = CommonIconSettings.Icon.None)
        {
            if(value != 0)
                Spawn(prefab, init: item => item.Init(worldPosition, RandomAdditionalDistance, colorLabel, value, icon));
        }

        public void Spawn(Vector3 worldPosition, Color colorLabel, string text, CommonIconSettings.Icon icon = CommonIconSettings.Icon.None)
        {
            if(!string.IsNullOrWhiteSpace(text))
                Spawn(prefab, init: item => item.Init(worldPosition, RandomAdditionalDistance, colorLabel, text, icon));
        }

        public Sprite GetIcon(CommonIconSettings.Icon icon) => commonIconSettings.GetIcon(icon);
    }
}
