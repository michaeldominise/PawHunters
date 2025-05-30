using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class EntityStatusEffectController : MonoBehaviour
    {
        [SerializeField, TableList] List<StatusEffectDataHandler> statusEffects;
        EntityMainController entityMainController;

        private void OnEnable()
        {
            GameActionTriggersManager.Instance.Register(TrigerExecute);
            GameActionTriggersManager.Instance.Register(TriggerExpire);
        }

        private void OnDisable()
        {
            GameActionTriggersManager.Instance.Unegister(TrigerExecute);
            GameActionTriggersManager.Instance.Unegister(TriggerExpire);
        }

        public void Init(EntityMainController entityMainController) => this.entityMainController = entityMainController;

        [Button]
        public async Task ApplyStatusEffect(StatusEffectData statusEffectData, EntityMainController caster)
        {
            var statusEffectDataHandler = new StatusEffectDataHandler(StatusEffectSpawner.Instance.Spawn(statusEffectData), caster, entityMainController);
            statusEffects.Add(statusEffectDataHandler);
            if (statusEffectDataHandler.data.ExecuteTrigger == GameActionTriggersManager.TriggerType.Instant)
                await statusEffectDataHandler.Execute();
            if (statusEffectDataHandler.data.ExpirationTrigger == GameActionTriggersManager.TriggerType.Instant)
                await statusEffectDataHandler.Expire();
        }

        [Button]
        public async Task TrigerExecute(GameActionTriggersManager.TriggerType triggerType, EntityMainController triggerSource = null)
        {
            foreach (var statusEffect in statusEffects)
            {
                if (!statusEffect.data.ExecuteTrigger.HasFlag(triggerType))
                    continue;
                await statusEffect.Execute();
            }
        }

        [Button]
        public async Task TriggerExpire(GameActionTriggersManager.TriggerType triggerType, EntityMainController triggerSource = null)
        {
            var statusEffectCopy = new List<StatusEffectDataHandler>(statusEffects);
            foreach (var statusEffect in statusEffectCopy)
            {
                if (!statusEffect.data.ExpirationTrigger.HasFlag(triggerType))
                    continue;

                await statusEffect.Expire();
                if (statusEffect.expirationCountdown <= 0)
                    RemoveStatusEffect(statusEffect);
            }
        }

        [Button]
        public void RemoveStatusEffect(StatusEffectDataHandler statusEffect)
        {
            statusEffects.Remove(statusEffect);
            StatusEffectSpawner.Instance.Despawn(statusEffect.data);
        }

        [Button]
        public void Clear()
        {
            var statusEffectCopy = new List<StatusEffectDataHandler>(statusEffects);
            foreach (var statusEffect in statusEffectCopy)
                RemoveStatusEffect(statusEffect);
        }
    }
}
