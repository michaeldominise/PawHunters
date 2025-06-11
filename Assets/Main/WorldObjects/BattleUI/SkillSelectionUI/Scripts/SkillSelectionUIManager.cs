using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class SkillSelectionUIManager : Spawner<SkillSelectionItem>
    {
        public static SkillSelectionUIManager Instance { get; private set; }

        [SerializeField] SkillSelectionItem prefab;
        [SerializeField] GameObject container;
        [SerializeField] ToggleGroup toggleGroup;
        [SerializeField] Button continueButton;

        Action<SkillData> onSkillSelected;
        SkillData SelectedSkillData => activeList.FirstOrDefault(x => x.IsSelected)?.skillData;

        private void Awake() => Instance = this;
        private void Start() => continueButton.onClick.AddListener(OnContinueClicked);

        public void Init(Action<SkillData> onSkillSelected, params SkillData[] skillDataList)
        {
            Clear();
            container.SetActive(true);
            this.onSkillSelected = onSkillSelected;
            continueButton.interactable = false;

            foreach (var skillData in skillDataList)
                Spawn(prefab, init: item => item.Init(skillData, toggleGroup, itemSelected => continueButton.interactable = SelectedSkillData));

            LayoutRebuilder.ForceRebuildLayoutImmediate(spawnParent.transform as RectTransform);
        }

        void OnContinueClicked()
        {
            onSkillSelected?.Invoke(SelectedSkillData);
            container.SetActive(false);
        }
    }
}
