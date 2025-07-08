using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class OverlayUI_Element : OverlayUI<ElementType>
    {
        AppSettings_Global.ColorTheme ColorTheme => AppSettings_Global.Instance.colorTheme;
        protected override Color GetBackgroundColor(ElementType value) => ColorTheme.elementColorTheme.GetColor(value);
        protected override Color GetForegroundColor(ElementType value) => ColorTheme.elementColorTheme_Background.GetColor(value);
    }
}
