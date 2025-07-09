using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class EntityStatsPreview : SingletonMonoBehaviour<EntityStatsPreview>
    {
        [SerializeField] GameObject container;
        [SerializeField] GameObject contents;
        [SerializeField] TextMeshProUGUI nameLabel;
        [SerializeField] EntityElementManager elementManager; 
        [SerializeField] EntityLevelManager levelManager;
        [SerializeField] EntityAssetManager assetManager;
        [SerializeField] StatsItemManager battleStats;
        [SerializeField] StatsItemManager elementStats;
        [SerializeField] SkillsPreview skillsPreview;
        [SerializeField] ScrollRect scrollRect;

        AppSettings_Global.IconSprites IconSprites => AppSettings_Global.Instance.iconSprites;
        AppSettings_Global.ConstantValues.BattleStatsMinMax BattleStatsMinMax => AppSettings_Global.Instance.constantValues.battleStatsMinMax;
        SaveableDataEntity data;
        Action onUpdate;

        public void Refresh() => Show(data, onUpdate);
        public async void Show(SaveableDataEntity data, Action onUpdate)
        {
            this.onUpdate = onUpdate;
            Show();

            Unload();
            this.data = SaveableData.Initialize(this.data, data, Refresh);
            await Load();

            contents.SetActive(true);
            nameLabel.text = data.Name;
            assetManager.Init(data);
            elementManager.Init(data);
            levelManager.Init(data);
            battleStats.Init(GetBattleStats());
            elementStats.Init(GetElementStats());
            skillsPreview.Init(data.SkillDataList);

            await Task.Yield();
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        }

        public List<StatsItem_Slider.DataSlider> GetBattleStats()
        {
            var statList = new List<StatsItem_Slider.DataSlider>
            {
                CreateDataSlider(SkillAttributeData.AttributeType.MaxHealth, data.Attribute.health),
                CreateDataSlider(SkillAttributeData.AttributeType.Attack, data.Attribute.attack),
                CreateDataSlider(SkillAttributeData.AttributeType.Defense, data.Attribute.defense),
                CreateDataSlider(SkillAttributeData.AttributeType.Speed, data.Attribute.speed),
                CreateDataSlider(SkillAttributeData.AttributeType.CritChance, data.Attribute.critChance, true),
                CreateDataSlider(SkillAttributeData.AttributeType.CritDamage, data.Attribute.critDamage, true)
            };

            return statList;
        }

        public StatsItem_Slider.DataSlider CreateDataSlider(SkillAttributeData.AttributeType attributeType, float value, bool isPercentage = false)
            => new(BattleStatsMinMax.GetNormalizedPercentatge(value, attributeType), attributeType.ToString().SeparateCamelCase(), isPercentage ? $"{value * 100:0}%" : $"{value}", IconSprites.battleStats.GetSprite(attributeType));

        public List<StatsItem_Element.DataElement> GetElementStats()
        {
            var statList = new List<StatsItem_Element.DataElement>();

            var descriptiveDict = new Dictionary<ElementType, (float effectivity, float resistance)>();
            foreach (var skill in data.SkillDataList)
            {
                foreach (var descriptive in skill.descriptives)
                {
                    var key = descriptive.elementType;
                    descriptiveDict.TryGetValue(key, out var tuple);

                    if (descriptive.key == SkillData.Descriptive.Key.ElementEffectivity)
                        tuple.effectivity += descriptive.value;
                    else if (descriptive.key == SkillData.Descriptive.Key.ElementResistance)
                        tuple.resistance += descriptive.value;

                    descriptiveDict[key] = tuple;
                }
            }

            foreach (var x in descriptiveDict)
                statList.Add(new(x.Key, x.Key.ToString(), $"{x.Value.effectivity * 100:0}%", $"{x.Value.resistance * 100:0}%", IconSprites.element.GetSprite(x.Key)));

            return statList;
        }

        async Task Load() => await data.LoadAssets(SaveableDataEntity.AssetType.All);
        void Unload() => data?.UnloadAssets(SaveableDataEntity.AssetType.All);

        public void Close() => BackNavigationHandler.Execute();

        public void Show(bool value = true)
        {
            scrollRect.verticalNormalizedPosition = 1;
            container.SetActive(value);
            contents.SetActive(false);
        }
    }
}
