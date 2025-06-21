using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class EntityFollowEntityWorldPosition : MonoBehaviour
    {
        [SerializeField] EntityUIWorld entityWorldUI;

        public EntityMainController EntityMainController => entityWorldUI.EntityMainController;
        Camera WorldCamera => Camera.main;
        RectTransform RectParent => transform.parent as RectTransform;

        private void LateUpdate()
        {
            if (!EntityMainController)
                return;
            var viewportPoint = WorldCamera.WorldToViewportPoint(EntityMainController.Anchor.worldUI.position);
            var halfScreenSize = new Vector3(RectParent.rect.width, RectParent.rect.height) * 0.5f;
            transform.localPosition = new Vector3(Mathf.LerpUnclamped(-halfScreenSize.x, halfScreenSize.x, viewportPoint.x), Mathf.LerpUnclamped(-halfScreenSize.y, halfScreenSize.y, viewportPoint.y));
        }
    }
}
