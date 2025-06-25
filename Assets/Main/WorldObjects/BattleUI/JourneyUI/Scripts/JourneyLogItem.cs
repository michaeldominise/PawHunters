using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class JourneyLogItem : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI day;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] AnimationUI bounceTransition;

        public async void Init(JourneyData.Type journeyType, string title, string description, string day = "")
        {
            this.day.gameObject.SetActive(!string.IsNullOrEmpty(day));
            this.day.text = day;
            this.description.text = description.Replace("[title]", $"<color=#{ColorUtility.ToHtmlStringRGB(GetColor(journeyType))}>{title}</color>");
            canvasGroup.alpha = 0;
            await Task.Delay(100);
            canvasGroup.alpha = 1;
            transform.localScale = Vector3.zero;
            await Task.Delay(100);
            bounceTransition.ScaleNormal();
        }

        public Color GetColor(JourneyData.Type journeyType)
            => journeyType switch
            {
                JourneyData.Type.Battle => AppSettings_Battle.Instance.colorTheme.journeyLogBattleColor,
                JourneyData.Type.Boss => AppSettings_Battle.Instance.colorTheme.journeyLogBossColor,
                JourneyData.Type.Negative => AppSettings_Battle.Instance.colorTheme.journeyLogNegativeColor,
                JourneyData.Type.Reward => AppSettings_Battle.Instance.colorTheme.journeyLogRewardColor,
                _ => AppSettings_Battle.Instance.colorTheme.journeyLogPositiveColor,
            };
    }
}
