using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using TMPro;

namespace LabHaven.PawHunters
{
    public class MenuItem_ColorChange : MenuItem
    {
        [SerializeField] List<MaskableGraphic> graphics;

        Color SelectedColor => AppSettings_Global.Instance.colorTheme.tabSelectedText;
        Color NotSelectedColor => AppSettings_Global.Instance.colorTheme.tabNotSelectedText;

        protected override void OnSelect()
        {
            graphics.ForEach(x => x.color = SelectedColor);
            base.OnSelect();
        }

        protected override void OnDeselect()
        {
            graphics.ForEach(x => x.color = NotSelectedColor);
            base.OnDeselect();
        }
    }
}
