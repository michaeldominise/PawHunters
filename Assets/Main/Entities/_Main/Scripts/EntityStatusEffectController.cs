using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class EntityStatusEffectController : MonoBehaviour
    {
        [SerializeField, TableList] List<StatusEffectDataHandler> statusEffects;
        EntityMainController entityMainController;

        private IEnumerator Start()
        {
            if (!GameActionTriggersManager.Instance)
                yield break;
            yield return null;
            GameActionTriggersManager.Instance.Register(TrigerExecute);
            GameActionTriggersManager.Instance.Register(TriggerExpire);
        }

        private void OnDestroy()
        {
            if (!GameActionTriggersManager.Instance)
                return;
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
                await ApplyExpiration(statusEffectDataHandler);
        }
        [Button]
        public async Task ApplyExpiration(StatusEffectDataHandler statusEffectDataHandler)
        {
            await statusEffectDataHandler.Expire();
            if (statusEffectDataHandler.expirationCountdown <= 0)
                RemoveStatusEffect(statusEffectDataHandler);
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
                await ApplyExpiration(statusEffect);
            }
        }

        [Button]
        public void RemoveStatusEffect(StatusEffectDataHandler statusEffect)
        {
            statusEffects.Remove(statusEffect);
            StatusEffectSpawner.Instance.Despawn(statusEffect.data);
        }

        public float GetEnhanceValue(EntityMainController target, StatusEffectData_Enhance.EnhanceType enhanceType)
        {
            var value = 1f;
            foreach (var statusEffect in statusEffects)
            {
                var statusEffect_Enhanced = statusEffect.data as StatusEffectData_Enhance;
                if (!statusEffect_Enhanced)
                    continue;

                value += statusEffect_Enhanced.CanEnhanceValue(target, enhanceType) ? statusEffect.cachedValue : 0;
            }
            return value;
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
