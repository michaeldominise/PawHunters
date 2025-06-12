using System;
using System.Collections.Generic;
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
        [SerializeField] int maxSkillCount = 3;

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

            var skills = GetRandom(skillDataList.ToList()).GetRange(0, maxSkillCount);
            foreach (var skillData in skills)
                Spawn(prefab, init: item => item.Init(skillData, toggleGroup, itemSelected => continueButton.interactable = SelectedSkillData));

            LayoutRebuilder.ForceRebuildLayoutImmediate(spawnParent.transform as RectTransform);
        }

        void OnContinueClicked()
        {
            onSkillSelected?.Invoke(SelectedSkillData);
            container.SetActive(false);
        }

        List<SkillData> GetRandom(List<SkillData> list)
        {
            var listCopy = new List<SkillData>(list);
            for (var x = 0; x < list.Count; x++)
            {
                var rnd = UnityEngine.Random.Range(0, listCopy.Count);
                list[x] = listCopy[rnd];
                listCopy.RemoveAt(rnd);
            }
            return list;
        }
    }
}
