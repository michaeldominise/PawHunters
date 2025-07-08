using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class StatsItem_Slider : StatsItem<StatsItem_Slider.DataSlider>
    {
        [System.Serializable]
        public class DataSlider : Data
        {
            public float sliderValue;

            public DataSlider(float sliderValue, string title, string value, Sprite sprite) : base(title, value, sprite)
                => this.sliderValue = sliderValue;
        }

        [SerializeField] Slider slider;

        public override void Init(DataSlider data)
        {
            base.Init(data);
            slider.value = data.sliderValue;
        }
    }
}
