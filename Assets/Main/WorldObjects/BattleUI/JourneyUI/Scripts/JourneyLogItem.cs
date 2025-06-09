using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class JourneyLogItem : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI day;
        [SerializeField] TextMeshProUGUI description;

        public void Init(JourneyData.Type journeyType, string title, string description, string day = "")
        {
            this.day.gameObject.SetActive(!string.IsNullOrEmpty(day));
            this.day.text = day;
            this.description.text = description.Replace("[title]", $"<color=#{ColorUtility.ToHtmlStringRGB(GetColor(journeyType))}>{title}</color>");
        }

        public Color GetColor(JourneyData.Type journeyType)
            => journeyType switch
            {
                JourneyData.Type.Battle => GameSettings_Battle.Instance.colorTheme.journeyLogBattleColor,
                JourneyData.Type.Boss => GameSettings_Battle.Instance.colorTheme.journeyLogBossColor,
                _ => GameSettings_Battle.Instance.colorTheme.journeyLogRewardColor,
            };
    }
}
