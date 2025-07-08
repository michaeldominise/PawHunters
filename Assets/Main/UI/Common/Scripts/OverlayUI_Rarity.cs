using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class OverlayUI_Rarity : OverlayUI<RarityType>
    {
        AppSettings_Global.ColorTheme ColorTheme => AppSettings_Global.Instance.colorTheme;
        protected override Color GetBackgroundColor(RarityType value) => ColorTheme.rarityColorTheme_Background.GetColor(value);
        protected override Color GetForegroundColor(RarityType value) => ColorTheme.rarityColorTheme.GetColor(value);
    }
}
