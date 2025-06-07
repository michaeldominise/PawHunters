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
            None,
            Instant = 1 << 0,
            BeginRound = 1 << 1,
            FinishRound = 1 << 2,
            ApplyCasterStatusEffect = 1 << 3,
            ApplyTargetStatusEffect = 1 << 4,
            InitiateBattle = 1 << 5,
            EndBattle = 1 << 6,
            SetupPhase = 1 << 7,
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
