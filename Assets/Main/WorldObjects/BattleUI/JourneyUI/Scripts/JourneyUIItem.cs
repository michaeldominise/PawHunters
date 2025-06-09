using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class JourneyUIItem : MonoBehaviour
    {
        public enum State { Inactive, Active, }

        [SerializeField] Image background;
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI textNum;

        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();
        Color JourneyInactiveColor => GameSettings_Battle.Instance.colorTheme.journeyInactiveColor;
        Color JourneyActiveColor => GetColor();
        JourneyData journeyData;

        public void Init(JourneyData journeyData, int index, State state = State.Inactive)
        {
            this.journeyData = journeyData;
            icon.sprite = GameSettings_Battle.Instance.iconSprite.GetSprite(GetIconType());
            textNum.text = $"{index + 1}";
            SetState(state);
        }

        [Button]
        public void SetState(State state)
        {
            background.color = state == State.Active ? JourneyActiveColor : JourneyInactiveColor;
            CurrentState.Value = state;
        }

        public GameSettings_Battle.Type GetIconType()
            => journeyData.JourneyType switch
            {
                JourneyData.Type.Battle => GameSettings_Battle.Type.JourneyBattle,
                JourneyData.Type.Boss => GameSettings_Battle.Type.JourneyBoss,
                _ => GameSettings_Battle.Type.JourneyDefault,
            };

        public Color GetColor()
            => journeyData.JourneyType switch
            {
                JourneyData.Type.Battle => GameSettings_Battle.Instance.colorTheme.journeyActiveColor_Battle,
                JourneyData.Type.Boss => GameSettings_Battle.Instance.colorTheme.journeyActiveColor_Boss,
                _ => GameSettings_Battle.Instance.colorTheme.journeyActiveColor_Default,
            };
    }
}
