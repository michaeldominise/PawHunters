using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class EntityHealthController : StateController<EntityHealthController.State>
    {
        public enum State { Alive, Dead }
        [SerializeField] int currentHealth;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => CharacterData.attribute.maxHealth;
        public float HealthProgress => (float)CurrentHealth / CharacterData.attribute.maxHealth;

        public event Action<int> OnHealthUpdate;

        EntityMainController playerMainController;
        CharacterData CharacterData => playerMainController.CharacterData;


        public void Init(EntityMainController playerMainController)
        {
            CurrentState = State.Alive;
            this.playerMainController = playerMainController;

            currentHealth = MaxHealth;
        }

        [Button]
        public void DoDamage(int value) => AddHealth(-value);
        public void AddHealth(int value)
        {
            if (CurrentState == State.Dead)
                return;

            currentHealth = Mathf.Clamp(currentHealth + value, 0, CharacterData.attribute.maxHealth);
            OnHealthUpdate?.Invoke(currentHealth);
            CurrentState = currentHealth > 0 ? State.Alive : State.Dead;
        }
    }
}
