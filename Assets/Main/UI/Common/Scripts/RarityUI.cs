using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class RarityUI : MonoBehaviour
    {
        public enum Type { Foreground, Background }

        [SerializeField] Type type;
        [SerializeField] List<Graphic> graphics;
        [SerializeField] float alpha = 1;

        AppSettings_Global.ColorTheme_Global ColorTheme => AppSettings_Global.Instance.colorTheme;

        [Button]
        public void Init(RarityType rarityType)
        {
            var color = type == Type.Foreground ? ColorTheme.rarityColorTheme.GetColor(rarityType) : ColorTheme.rarityColorTheme_Background.GetColor(rarityType);
            color.a = alpha;
            graphics.ForEach(x => x.color = color);
        }
    }
}
