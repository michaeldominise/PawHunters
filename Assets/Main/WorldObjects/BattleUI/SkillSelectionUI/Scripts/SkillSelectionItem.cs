using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class SkillSelectionItem : MonoBehaviour
    {
        [SerializeField] Toggle toggle;
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] TextMeshProUGUI rarity;

        public SkillData skillData;

        Action<SkillSelectionItem> onSelect;

        public bool IsSelected => toggle.isOn;

        Color RarityColor => AppSettings_Global.Instance.colorTheme.rarityColorTheme.GetColor(skillData.rarity);

        private void Start() => toggle.onValueChanged.AddListener(OnToggleClick);

        public void Init(SkillData skillData, ToggleGroup toggleGroup, Action<SkillSelectionItem> onSelect = null)
        {
            this.skillData = skillData;
            this.onSelect = onSelect;
            toggle.group = toggleGroup;
            toggle.isOn = false;
            icon.sprite = skillData.icon;
            icon.gameObject.SetActive(skillData.icon);
            title.text = skillData.title;
            description.text = skillData.description;
            rarity.text = $"<color=#{ColorUtility.ToHtmlStringRGB(RarityColor)}>{skillData.rarity}</color>";
        }

        void OnToggleClick(bool value) => onSelect?.Invoke(this);
    }
}
