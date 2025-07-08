using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class JourneyUIItem : MonoBehaviour
    {
        public enum State { Inactive, Active, }

        [SerializeField] Image background;
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI textNum;

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        Color JourneyInactiveColor => AppSettings_Battle.Instance.colorTheme.journeyInactiveColor;
        Color JourneyActiveColor => GetColor();
        JourneyData journeyData;

        public void Init(JourneyData journeyData, int index, State state = State.Inactive)
        {
            this.journeyData = journeyData;
            icon.sprite = AppSettings_Battle.Instance.iconSprites.GetSprite(GetIconType());
            textNum.text = $"{index + 1}";
            SetState(state);

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        [Button]
        public void SetState(State state)
        {
            background.color = state == State.Active ? JourneyActiveColor : JourneyInactiveColor;
            CurrentState.Value = state;
        }

        public AppSettings_Battle.IconSprites.Type GetIconType()
            => journeyData.JourneyType switch
            {
                JourneyData.Type.Battle => AppSettings_Battle.IconSprites.Type.JourneyBattle,
                JourneyData.Type.Boss => AppSettings_Battle.IconSprites.Type.JourneyBoss,
                _ => AppSettings_Battle.IconSprites.Type.JourneyDefault,
            };

        public Color GetColor()
            => journeyData.JourneyType switch
            {
                JourneyData.Type.Battle => AppSettings_Battle.Instance.colorTheme.journeyActiveColor_Battle,
                JourneyData.Type.Boss => AppSettings_Battle.Instance.colorTheme.journeyActiveColor_Boss,
                _ => AppSettings_Battle.Instance.colorTheme.journeyActiveColor_Default,
            };
    }
}
