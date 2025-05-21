using Assets.FantasyMonsters.Common.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PawHunters
{
    public abstract class EntityMovementController : MonoBehaviour
    {
        public enum State { Idle, Moving }

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        protected EntityMainController playerMainController;

        public virtual void Init(EntityMainController playerMainController) => this.playerMainController = playerMainController;
    }
}
