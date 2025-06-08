using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace PawHunters
{
    public abstract class StatusEffectData : MonoBehaviour
    {
        public enum ModifierType { Add, Subtract }
        public enum ExpireActionType { None, RevertStatusEffects }

        [System.Serializable]
        public class AttributeModifiers
        {
            public enum ModifierType { Add, Subtract, Divide, Multiply }
            public enum SourceType { Target, Caster }

            [SerializeField] ModifierType modifierType;
            [SerializeField] SkillAttributeData.AttributeType attributeData;
            [SerializeField] SourceType sourceType;

            public float GetValue(float referenceValue, EntityMainController caster, EntityMainController target)
            {
                var getValueTarget = sourceType == SourceType.Caster ? caster : target;
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

        [SerializeField, FoldoutGroup("Basic")] string title;
        [SerializeField, FoldoutGroup("Basic"), TextArea] string description;
        [SerializeField, FoldoutGroup("Basic")] Sprite icon;
        [SerializeField, FoldoutGroup("Basic")] ElementType elementType;
        [SerializeField, FoldoutGroup("Basic")] StatusEffectTags tags;
        [SerializeField, FoldoutGroup("Basic")] bool showStatusTextUI;

        [SerializeField, FoldoutGroup("Value and Computation")] float baseValue;
        [SerializeField, FoldoutGroup("Value and Computation"), TableList, FormerlySerializedAs("attributeModifiers")] List<AttributeModifiers> valueModifiers;
        [SerializeField, FoldoutGroup("Value and Computation")] ModifierType modifierType;

        [SerializeField, FoldoutGroup("Execution")] GameActionTriggersManager.TriggerType executeTrigger = GameActionTriggersManager.TriggerType.Instant;
        [SerializeField, FoldoutGroup("Execution")] SkillVisualEffect[] executeSkillVFXs;
        [SerializeField, FoldoutGroup("Execution")] float executeFinishDelay = 0.25f;
        [SerializeField, FoldoutGroup("Execution")] SkillTargetData.HealthStatusType targetHealthStatus = SkillTargetData.HealthStatusType.Alive;
        [SerializeField, FoldoutGroup("Execution"), FormerlySerializedAs("customConditions")] List<SkillCustomCondition> executeCustomConditions;

        [SerializeField, FoldoutGroup("Expiration")] GameActionTriggersManager.TriggerType expirationTrigger = GameActionTriggersManager.TriggerType.Instant;
        [SerializeField, FoldoutGroup("Expiration")] ExpireActionType expireAction;
        [SerializeField, FoldoutGroup("Expiration")] int expirationCount = 0;
        [SerializeField, FoldoutGroup("Expiration")] SkillVisualEffect[] expireCountdownSkillVFX;
        [SerializeField, FoldoutGroup("Expiration")] SkillVisualEffect[] expireSkillVFXs;
        [SerializeField, FoldoutGroup("Expiration")] float expireFinishDelay = 0f;
        [SerializeField, FoldoutGroup("Expiration")] List<SkillCustomCondition> expireCustomConditions;

        public string Title => title;
        public string Description => description;
        public Sprite Icon => icon;
        public ElementType Element => elementType;
        public StatusEffectTags Tags => tags;

        public GameActionTriggersManager.TriggerType ExecuteTrigger => executeTrigger;
        public int ExpirationCount => expirationCount;
        public SkillTargetData.HealthStatusType TargetHealthStatus => targetHealthStatus;
        public List<SkillCustomCondition> ExecuteCustomCondition => executeCustomConditions;

        public GameActionTriggersManager.TriggerType ExpirationTrigger => expirationTrigger;
        public ExpireActionType ExpireAction => expireAction;
        public List<SkillCustomCondition> ExpireCustomCondition => expireCustomConditions;

        public float GetValue(EntityMainController caster, EntityMainController target)
        {
            var value = baseValue;
            foreach (var attributeModifier in valueModifiers)
                value = attributeModifier.GetValue(value, caster, target);
            return modifierType == ModifierType.Add ? value : -value;
        }

        public virtual async Task WaitStopMoving(StatusEffectDataHandler statusEffectDataHandler)
        {
            while (statusEffectDataHandler.caster.EntityMovementController.CurrentState.Value == EntityMovementController.State.Moving)
                await Task.Yield();
        }

        public virtual async Task<float> Execute(StatusEffectDataHandler statusEffectDataHandler)
        {
            await Task.Delay((int)(executeFinishDelay * 1000));
            return 0;
        }

        public virtual async Task PlayExecuteVisual(SkillVisualEffect.State state, StatusEffectDataHandler statusEffectDataHandler)
            => await SkillVisualEffect.PlayVisual(state, executeSkillVFXs, statusEffectDataHandler);

        public virtual async Task Expire(StatusEffectDataHandler statusEffectDataHandler) => await Task.Delay((int)(expireFinishDelay * 1000));

        public virtual async Task PlayExpireCountdownVisual(SkillVisualEffect.State state, StatusEffectDataHandler statusEffectDataHandler)
            => await SkillVisualEffect.PlayVisual(state, expireCountdownSkillVFX, statusEffectDataHandler);

        public virtual async Task PlayExpireDoneVisual(SkillVisualEffect.State state, StatusEffectDataHandler statusEffectDataHandler)
            => await SkillVisualEffect.PlayVisual(state, expireSkillVFXs, statusEffectDataHandler);

        protected void ShowStatusTextUI(EntityMainController target, Color color, float value, GameSettings_Battle.Type icon = GameSettings_Battle.Type.None)
        {
            if (!showStatusTextUI)
                return;

            StatusTextUISpawner.Instance.Spawn(target.Anchor.statusTextUI.position, color, value, icon);
        }
    }
}
