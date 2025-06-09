using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class EntityHealthController : MonoBehaviour
    {
        public enum State { Alive, Dead }

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

        EntityMainController entityMainController;
        BattleAttributes BattleAttributes => entityMainController.BattleAttributes;

        public void Init(EntityMainController entityMainController)
        {
            CurrentState.Value = State.Alive;
            this.entityMainController = entityMainController;
        }

        [Button]
        public void AddHealth(float value, object obj = null)
        {
            if (CurrentState.Value == State.Dead)
                return;
            if (value < 0)
                entityMainController.EntityAnimationController.SetState(EntityAnimationController.State.Hurt);
            BattleAttributes.currentHealth.Update(value, obj, currentValue => Math.Clamp(value, -BattleAttributes.currentHealth.Value, BattleAttributes.maxHealth.Value - currentValue));
            CurrentState.Value = BattleAttributes.currentHealth.Value > 0 ? State.Alive : State.Dead;
        }

        [Button]
        public void AddMaxHealth(float value, object obj = null)
        {
            var healthPercentage = BattleAttributes.HealthPercentage;
            BattleAttributes.maxHealth.Update(value, obj);
            BattleAttributes.currentHealth.Update(BattleAttributes.maxHealth.Value * healthPercentage - BattleAttributes.currentHealth.Value, obj);
        }

        [Button]
        public void AddSheild(float value, object obj = null) => BattleAttributes.shield.Update(value, obj, currentValue => Math.Max(value, -currentValue));

        [Button]
        public void Kill() => AddHealth(-BattleAttributes.currentHealth.Value);
    }
}
