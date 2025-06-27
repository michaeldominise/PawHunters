using DG.Tweening;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class TeamManger_EntityParent : MonoBehaviour
    {
        public EntityMainController entityMainController;
        int Index => transform.GetSiblingIndex();

        public void Init(EntityMainController entityMainController)
        {
            this.entityMainController = entityMainController;
            if (!entityMainController)
                return;
            entityMainController.transform.SetParent(transform);
            entityMainController.transform.localPosition = Vector3.zero;
            entityMainController.transform.localRotation = Quaternion.identity;
            entityMainController.transform.localScale = entityMainController.CharacterData.GetPrefab().transform.localScale;
            entityMainController.transform.DOLocalJump(Vector3.zero, 0.2f, 1, 0.25f).SetDelay(Index * 0.05f);
        }
    }
}
