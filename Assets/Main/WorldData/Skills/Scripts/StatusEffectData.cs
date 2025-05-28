using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public abstract class StatusEffectData : MonoBehaviour
    {
        public enum ModifierType { Add, Subtract }
        public enum ExecuteTargetType { Target, Caster, TriggerSource }
        public enum ExpireActionType { None, RevertStatusEffects }

        [System.Flags] public enum TriggerType
        {
            Instant = 0,
            StartRound = 1 << 1,
            EndRound = 1 << 2,
            WhenAttacked = 1 << 3,
            WhenAttacking = 1 << 4,
        }

        [System.Serializable]
        public class AttributeModifiers
        {
            public enum ModifierType { Add, Subtract, Divide, Multiply }
            public enum SourceType { Caster, Target, TriggerSource }

            [SerializeField] SourceType sourceType;
            [SerializeField] SkillAttributeData.AttributeType attributeData;
            [SerializeField] ModifierType modifierType;

            public float GetValue(float referenceValue, EntityMainController caster, EntityMainController target, EntityMainController triggerSource)
            {
                var getValueTarget = sourceType switch
                {
                    SourceType.Target => target,
                    SourceType.TriggerSource => triggerSource,
                    _ => caster
                };
                var attributeValue = attributeData.GetValue(getValueTarget);
                return modifierType switch
                {
                    ModifierType.Subtract => referenceValue - attributeValue,
                    ModifierType.Divide => referenceValue / attributeValue,
                    ModifierType.Multiply => referenceValue * attributeValue,
                    _ => referenceValue + attributeValue,
                };
            }
        }

        [SerializeField, TextArea] string description;
        [SerializeField] Sprite icon;
        [SerializeField] float baseValue;
        [SerializeField, TableList] List<AttributeModifiers> attributeModifiers;
        [SerializeField] ModifierType modifierType;
        [SerializeField] int expirationCount = 0;
        [SerializeField] TriggerType executeTrigger;
        [SerializeField] TriggerType expirationTrigger;
        [SerializeField] ExpireActionType expireAction;
        [SerializeField] ExecuteTargetType executeTo;
        [SerializeField] bool refreshValueOnExecute;
        [SerializeField] bool runToTargetBeforeCast;

        public string Description => description;
        public Sprite Icon => icon;
        public int ExpirationCount => expirationCount;
        public TriggerType ExecuteTrigger => executeTrigger;
        public TriggerType ExpirationTrigger => expirationTrigger;
        public ExpireActionType ExpireAction => expireAction;
        public ExecuteTargetType ExecuteTo => executeTo;
        public bool RefreshValueOnExecute => refreshValueOnExecute;
        public bool RunToTargetBeforeCast => runToTargetBeforeCast;

        public float GetValue(EntityMainController caster, EntityMainController target, EntityMainController triggerSource)
        {
            var value = baseValue;
            foreach (var attributeModifier in attributeModifiers)
                value = attributeModifier.GetValue(value, caster, target, triggerSource);
            return modifierType == ModifierType.Add ? value : -value;
        }

        public virtual async Task Execute(StatusEffectDataHandler statusEffectDataHandler)
        {
            while(statusEffectDataHandler.caster.EntityMovementController.CurrentState.Value == EntityMovementController.State.Moving)
                await Task.Yield();
        }

        public virtual async Task Expire(StatusEffectDataHandler statusEffectDataHandler) => await Task.Yield();

        public virtual async Task PlayExecuteVisual(StatusEffectDataHandler statusEffectDataHandler)
        {
            var isAttackDone = false;

            if(RunToTargetBeforeCast)
                await statusEffectDataHandler.caster.EntityMovementController.MoveToFront(statusEffectDataHandler.ExecuteToEntity, true);

            void OnAnimationStateUpdate(EntityAnimationController.State state) => isAttackDone = state == EntityAnimationController.State.AttackDone;

            statusEffectDataHandler.caster.EntityAnimationController.SetState(EntityAnimationController.State.Attacking);
            statusEffectDataHandler.caster.EntityAnimationController.CurrentState.RegisterListener(OnAnimationStateUpdate);
            while (!isAttackDone)
                await Task.Yield();
            statusEffectDataHandler.caster.EntityAnimationController.CurrentState.UnregisterListener(OnAnimationStateUpdate);

            if (RunToTargetBeforeCast)
                _ = statusEffectDataHandler.caster.EntityMovementController.MoveToStartPosition();
        }

        public virtual async Task PlayExpireCountdownVisual(StatusEffectDataHandler statusEffectDataHandler) => await Task.Yield();
        public virtual async Task PlayExpireDoneVisual(StatusEffectDataHandler statusEffectDataHandler) => await Task.Yield();
    }
}
