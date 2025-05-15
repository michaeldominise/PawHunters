using Assets.FantasyMonsters.Common.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PawHunters
{
    public abstract class EntityMovementController : StateController<EntityMovementController.State>
    {
        public enum State { Idle, Moving }

        protected EntityMainController playerMainController;

        public virtual void Init(EntityMainController playerMainController) => this.playerMainController = playerMainController;
    }
}
