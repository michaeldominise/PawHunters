using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class EntityUIWorld : MonoBehaviour
    {
        public enum State { Alive, Dead }

        [SerializeField] EntityMainController entityMainController;
        [SerializeField] EntityUIHealthBar entityUIHealthBar;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

        public EntityMainController EntityMainController => entityMainController;

        public void Init(EntityMainController entityMainController)
        {
            gameObject.SetActive(true);
            this.entityMainController = entityMainController;
            entityUIHealthBar.Init(entityMainController);
            CurrentState.Value = State.Alive;
        }

        public void Kill()
        {
            if (CurrentState.Value == State.Dead)
                return;
            StartCoroutine(_Kill());
        }

        IEnumerator _Kill()
        {
            CurrentState.Value = State.Dead;
            yield return new WaitForSeconds(entityUIHealthBar.UpdateDuration);
            gameObject.SetActive(false);
        }
    }
}
