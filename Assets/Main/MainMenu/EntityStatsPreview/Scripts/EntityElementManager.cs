using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class EntityElementManager : MonoBehaviour
    {
        [SerializeField] Image elementIcon;
        [SerializeField] TextMeshProUGUI elementLabel;
        [SerializeField] OverlayUI_Element overlayUI;

        public void Init(SaveableDataEntity saveableDataEntity)
        {
            elementIcon.sprite = AppSettings_Global.Instance.iconSprites.element.GetSprite(saveableDataEntity.ElementType);
            elementLabel.text = saveableDataEntity.ElementType.ToString().SeparateCamelCase();
            overlayUI.Init(saveableDataEntity.ElementType);

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
