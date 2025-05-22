using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class EntityHealthController : MonoBehaviour
    {
        public enum State { Alive, Dead }

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        [SerializeField] int currentHealth;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => CharacterData.attribute.maxHealth;
        public float HealthProgress => (float)CurrentHealth / CharacterData.attribute.maxHealth;

        public event Action OnHealthUpdate;

        EntityMainController playerMainController;
        SaveableCharacterData CharacterData => playerMainController.CharacterData;

        public void Init(EntityMainController playerMainController)
        {
            CurrentState.Value = State.Alive;
            this.playerMainController = playerMainController;
            currentHealth = 0;
            AddHealth(MaxHealth);
        }

        [Button]
        public void AddHealth(int value)
        {
            if (CurrentState.Value == State.Dead)
                return;

            currentHealth = Mathf.Clamp(currentHealth + value, 0, CharacterData.attribute.maxHealth);
            OnHealthUpdate?.Invoke();
            CurrentState.Value = currentHealth > 0 ? State.Alive : State.Dead;
        }

        [Button]
        public void DoDamage(int value) => AddHealth(-value);

        [Button]
        public void Kill() => AddHealth(-currentHealth);
    }
}
