using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class EntityUIWorld : MonoBehaviour
    {
        public enum State { Alive, Dead }

        [SerializeField] EntityMainController entityMainController;
        [SerializeField] EntityUIHealthBar entityUIHealthBar;
        [SerializeField] EntityUIShieldBar entityUIShieldBar;
        [SerializeField] EntityUISpecialSkillBar entityUISpecialSkillBar;
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

        public EntityMainController EntityMainController => entityMainController;

        public void Init(EntityMainController entityMainController)
        {
            if(this.entityMainController)
                this.entityMainController.CurrentState.UnregisterListener(PlayerMainController_OnStateUpdate);
            this.entityMainController = entityMainController;
            this.entityMainController.CurrentState.RegisterListener(PlayerMainController_OnStateUpdate);

            entityUIHealthBar.Init(entityMainController);
            entityUIShieldBar.Init(entityMainController);
            entityUISpecialSkillBar.Init(entityMainController);
            CurrentState.Value = State.Alive;
        }

        private void PlayerMainController_OnStateUpdate(EntityMainController.State state)
        {
            if (state == EntityMainController.State.Dead)
                Kill();
        }

        [Button]
        public void Kill()
        {
            if (CurrentState.Value == State.Dead)
                return;
            StartCoroutine(_Kill());
        }

        IEnumerator _Kill()
        {
            CurrentState.Value = State.Dead;
            yield return new WaitForSeconds(AppSettings_Battle.Instance.constantValues.progressUpdateDuration);
            gameObject.SetActive(false);
        }
    }
}
