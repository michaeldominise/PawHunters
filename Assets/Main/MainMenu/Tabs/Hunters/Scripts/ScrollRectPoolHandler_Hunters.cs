using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class ScrollRectPoolHandler_Hunters : ScrollRectPoolHandler<HunterPreviewItem, SaveableCharacterData>
    {
        public enum FilterType { Level, Rarity, DateCreated, DateOwned }
        public enum OrderType { Acending, Decending }

        [SerializeField] FilterType filterType;
        [SerializeField] OrderType orderType = OrderType.Decending;

        [Button]
        protected void SetSortedList(FilterType filterType, OrderType orderType)
        {
            this.filterType = filterType;
            this.orderType = orderType;
            SetSortedList();
        }

        protected override void SetSortedList()
            => SetSortedList(orderType == OrderType.Acending
                ? data.items.OrderBy(x => GetFilterValue(x)).ToList()
                : data.items.OrderByDescending(x => GetFilterValue(x)).ToList());

        public object GetFilterValue(SaveableCharacterData data)
        {
            object value = filterType switch
            {
                FilterType.Level => data.level,
                FilterType.Rarity => data.Rarity,
                FilterType.DateCreated => data.instanceData.dateCreatedString,
                FilterType.DateOwned => data.instanceData.dateOwnedString,
                _ => 0,
            };
            return value;
        }
    }
}
