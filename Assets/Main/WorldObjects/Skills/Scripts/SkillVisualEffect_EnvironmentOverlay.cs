using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class SkillVisualEffect_EnvironmentElementOverlay : SkillVisualEffect
    {
        [SerializeField] ElementType elementType;

        float FadeDuration => GameSettings_Battle.Instance.constantValues.environmentOverlayFadeDuration;
        float FadeOpacity => GameSettings_Battle.Instance.constantValues.environmentOverlayFadeOpacity;

        public override async Task PlayStart(EntityMainController caster, params EntityMainController[] targets)
            => EnvironmentManager.Instance.EnvironmentItem.ShowOverlay(GameSettings_Battle.Instance.colorTheme.elementColorOverlay.GetColor(elementType), FadeOpacity, FadeDuration);

        public override async Task PlayEnd(EntityMainController caster, params EntityMainController[] targets)
            => EnvironmentManager.Instance.EnvironmentItem.ShowOverlay(GameSettings_Battle.Instance.colorTheme.elementColorOverlay.GetColor(elementType), 0, FadeDuration);
    }
}
