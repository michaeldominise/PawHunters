using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public abstract class StatsItem : MonoBehaviour
    {
        [System.Serializable]
        public class Data
        {
            public string title;
            public string value;
            public Sprite icon;

            public Data(string title, string value, Sprite icon) => Init(title, value, icon);
            public Data Init(string title, string value, Sprite icon)
            {
                this.title = title;
                this.value = value;
                this.icon = icon;
                return this;
            }
        }

        public abstract void Init(Data data);
    }

    public abstract class StatsItem<T> : StatsItem where T : StatsItem.Data
    {
        [SerializeField] protected Image icon;
        [SerializeField] protected TextMeshProUGUI title;
        [SerializeField] protected TextMeshProUGUI value;

        protected T data;

        public override void Init(Data data) => Init(data as T);
        public virtual void Init(T data)
        {
            this.data = data;
            icon.sprite = data.icon;
            title.text = data.title;
            value.text = data.value;
            name = $"StatsItem_{data.title}";
        }
    }
}
