using System;
using System.Threading.Tasks;
using UnityEngine;

namespace PawHunters
{
    public class GameActionTriggersManager : MonoBehaviour
    {
        public static GameActionTriggersManager Instance { get; private set; }

        public event Func<StatusEffectData.TriggerType, EntityMainController, Task> OnTrigger;

        private void Awake() => Instance = this;

        public async Task ExecuteOnTrigger(StatusEffectData.TriggerType triggerType, EntityMainController triggerSource = null) => await OnTrigger?.Invoke(triggerType, triggerSource);
    }
}
