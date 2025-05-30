using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace PawHunters
{
    public class GameActionTriggersManager : MonoBehaviour
    {
        [Flags]
        public enum TriggerType
        {
            Instant = 1 << 0,
            StartRound = 1 << 1,
            EndRound = 1 << 2,
            StatusEffectExecuted = 1 << 3,
            StatusEffectExecutedToTarget = 1 << 4,
        }


        public static GameActionTriggersManager Instance { get; private set; }

        List<Func<TriggerType, EntityMainController, Task>> onTriggerList = new();

        private void Awake() => Instance = this;

        public async Task ExecuteOnTrigger(TriggerType triggerType, EntityMainController triggerSource = null)
        {
            foreach(var onTrigger in onTriggerList)
                await onTrigger?.Invoke(triggerType, triggerSource);
        }

        public void Register(Func<TriggerType, EntityMainController, Task> onTrigger)
        {
            if (onTriggerList.Contains(onTrigger))
                return;
            onTriggerList.Add(onTrigger);
        }

        public void Unegister(Func<TriggerType, EntityMainController, Task> onTrigger)
        {
            if (onTriggerList.Contains(onTrigger))
                return;
            onTriggerList.Add(onTrigger);
        }
    }
}
