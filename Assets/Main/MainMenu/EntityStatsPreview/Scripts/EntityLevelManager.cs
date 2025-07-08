using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class EntityLevelManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelLabel;
        [SerializeField] TextMeshProUGUI percentageLabel;
        [SerializeField] TextMeshProUGUI rarityLabel;
        [SerializeField] Slider slider;
        [SerializeField] OverlayUI_Rarity overlayUI;

        public void Init(SaveableDataEntity saveableDataEntity)
        {
            var rarity = saveableDataEntity.Rarity;
            var lowestLevel = rarity.RarityLowestLevel();
            var highestLevel = rarity.RarityHighestLevel();
            var sliderValue = Mathf.Clamp((float)(saveableDataEntity.level - lowestLevel) / (highestLevel - lowestLevel), 0.04f, 1);
            slider.value = sliderValue;
            levelLabel.text = $"{saveableDataEntity.level - lowestLevel}";
            percentageLabel.text = $"{sliderValue * 100:0}%";
            rarityLabel.text = rarity.ToString().ToUpper();
            overlayUI.Init(saveableDataEntity.Rarity);
        }
    }
}
