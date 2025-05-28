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
            GameActionTriggersManager.Instance.OnTrigger += TrigerExecute;
            GameActionTriggersManager.Instance.OnTrigger += TriggerExpire;
        }

        private void OnDisable()
        {
            GameActionTriggersManager.Instance.OnTrigger -= TrigerExecute;
            GameActionTriggersManager.Instance.OnTrigger -= TriggerExpire;
        }

        public void Init(EntityMainController entityMainController) => this.entityMainController = entityMainController;

        [Button]
        public async Task ApplyStatusEffect(StatusEffectData statusEffectData, EntityMainController caster)
        {
            var statusEffect = new StatusEffectDataHandler(StatusEffectSpawner.Instance.Spawn(statusEffectData), caster, entityMainController);
            statusEffects.Add(statusEffect);
            if (statusEffect.data.ExecuteTrigger == StatusEffectData.TriggerType.Instant)
                await statusEffect.Execute();
            if (statusEffect.data.ExpirationTrigger == StatusEffectData.TriggerType.Instant)
                await statusEffect.Expire();
        }

        [Button]
        public async Task TrigerExecute(StatusEffectData.TriggerType triggerType, EntityMainController triggerSource = null)
        {
            foreach (var statusEffect in statusEffects)
            {
                if (!statusEffect.data.ExecuteTrigger.HasFlag(triggerType))
                    continue;

                statusEffect.triggerSource = triggerSource;
                await statusEffect.Execute();
            }
        }

        [Button]
        public async Task TriggerExpire(StatusEffectData.TriggerType triggerType, EntityMainController triggerSource = null)
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
