using System.Collections;
using UnityEngine;

namespace PawHunters
{
    public class EntityMovementController_FollowCamera : EntityMovementController
    {
        IEnumerator Start()
        {
            yield return null;
            EnvironmentManager.Instance.OnMoveCamera += OnMoveCamera;
        }

        private void OnDestroy() => EnvironmentManager.Instance.OnMoveCamera -= OnMoveCamera;

        private void OnMoveCamera(Vector3 movement)
        {
            if (playerMainController.CharacterData == null)
                return;

            transform.position = new Vector3(EnvironmentManager.Instance.InitialCharacterPosition.x, EnvironmentManager.Instance.InitialCharacterPosition.y, transform.position.z);
            CurrentState = movement.sqrMagnitude > 0 ? State.Moving : State.Idle;
        }
    }
}
