
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public abstract class OverlayUI<T> : MonoBehaviour where T : System.Enum
    {
        public enum Type { Foreground, Background }

        [SerializeField] Type type;
        [SerializeField] List<Graphic> graphics;
        [SerializeField] float alpha = 1;

        [Button]
        public void Init(T value)
        {
            var color = type == Type.Foreground ? GetForegroundColor(value) : GetBackgroundColor(value);
            color.a = alpha;
            graphics.ForEach(x => x.color = color);
        }

        protected abstract Color GetBackgroundColor(T value);
        protected abstract Color GetForegroundColor(T value);
    }
}
