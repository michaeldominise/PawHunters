using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class StatsItem_Element : StatsItem<StatsItem_Element.DataElement>
    {
        [System.Serializable]
        public class DataElement : Data
        {
            public ElementType elementType;
            public string value2;

            public DataElement(ElementType elementType, string title, string value1, string value2, Sprite sprite) : base(title, value1, sprite)
            {
                this.elementType = elementType;
                this.value2 = value2;
            }
        }

        [SerializeField] List<OverlayUI_Element> overlayUIList;
        [SerializeField] protected TextMeshProUGUI value2;

        public override void Init(DataElement data)
        {
            base.Init(data);
            overlayUIList.ForEach(x => x.Init(data.elementType));
        }
    }
}
