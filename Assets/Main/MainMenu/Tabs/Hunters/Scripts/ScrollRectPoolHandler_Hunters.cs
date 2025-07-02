using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class ScrollRectPoolHandler_Hunters : ScrollRectPoolHandlerSorted<HunterPreviewItem, SaveableCharacterData, ScrollRectPoolHandler_Hunters.FilterType>
    {
        public enum FilterType { Level, Element, DateCreated, DateOwned }

        public Action<EntityPreviewItem<SaveableCharacterData>> onItemLoaded;
        public Action<EntityPreviewItem<SaveableCharacterData>> onItemClick;

        List<KeyValuePair<FilterType, OrderType>> defaultFilters = new()
        {
            new(FilterType.Level, OrderType.Decending),
            new(FilterType.Element, OrderType.Acending),
            new(FilterType.DateCreated, OrderType.Decending),
            new(FilterType.DateOwned, OrderType.Decending),
        };
        protected override List<KeyValuePair<FilterType, OrderType>> DefaultFilters => defaultFilters;

        public override object GetFilterValue(SaveableCharacterData data, FilterType filter)
        {
            object value = filter switch
            {
                FilterType.Level => data.level,
                FilterType.Element => (CharacterEntityOverview.Instance.GetAsset(data.MasterID) as CharacterEntityOverview.AssetReferenceMasterID).elementType,
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
