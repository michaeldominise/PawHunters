using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    [RequireComponent(typeof(Button))]
    public class ConditionalButton : MonoBehaviour
    {
        public enum ButtonType { Yes, No, Cancel }

        [SerializeField] ButtonType buttonType;
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI label;

        public void Init(Action<ButtonType> onClick, string buttonLabel)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke(buttonType));
            label.text = buttonLabel;
        }

        private void Reset()
        {
            button = GetComponent<Button>();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
