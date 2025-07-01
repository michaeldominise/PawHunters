using System;
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

        public Action<EntityPreviewItem<SaveableCharacterData>> onItemLoaded;
        public Action<EntityPreviewItem<SaveableCharacterData>> onItemClick;

        public override void SetSortedList()
            => SetSortedList(orderType == OrderType.Acending
                ? data.items.OrderBy(x => GetFilterValue(x)).ToList()
                : data.items.OrderByDescending(x => GetFilterValue(x)).ToList());

        [Button]
        public void SetSortedList(FilterType filterType, OrderType orderType)
        {
            this.filterType = filterType;
            this.orderType = orderType;
            SetSortedList();
        }

        public object GetFilterValue(SaveableCharacterData data)
        {
            object value = filterType switch
            {
                FilterType.Level => data.level,
                FilterType.Rarity => data.Rarity,
                FilterType.DateCreated => data.InstanceData.dateCreatedString,
                FilterType.DateOwned => data.InstanceData.dateOwnedString,
                _ => 0,
            };
            return value;
        }

        protected override void InitItem(int index, HunterPreviewItem item)
        {
            base.InitItem(index, item);
            item.onClick = onItemClick;
            item.onLoaded = onItemLoaded;
        }
    }
}
