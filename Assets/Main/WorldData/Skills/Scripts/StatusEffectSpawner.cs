using UnityEngine;

namespace PawHunters
{
    public class StatusEffectSpawner : Spawner<StatusEffectData>
    {
        public static StatusEffectSpawner Instance { get; private set; }

        private void Awake() => Instance = this;
    }
}
