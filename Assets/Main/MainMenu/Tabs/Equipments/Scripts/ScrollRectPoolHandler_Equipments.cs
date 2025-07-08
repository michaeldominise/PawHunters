using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class ScrollRectPoolHandler_Equipments : ScrollRectPoolHandlerSorted<EquipmentPreviewItem, SaveableEquipmentData, ScrollRectPoolHandler_Equipments.FilterType>
    {
        public enum FilterType { Level, Category, Element, DateCreated, DateOwned, Name }

        public Action<EntityPreviewItem<SaveableEquipmentData>> onItemLoaded;

        List<KeyValuePair<FilterType, OrderType>> defaultFilters = new()
        {
            new(FilterType.Level, OrderType.Decending),
            new(FilterType.Category, OrderType.Acending),
            new(FilterType.Element, OrderType.Acending),
            new(FilterType.DateCreated, OrderType.Decending),
            new(FilterType.DateOwned, OrderType.Decending),
            new(FilterType.Name, OrderType.Acending),
        };
        protected override List<KeyValuePair<FilterType, OrderType>> DefaultFilters => defaultFilters;

        public override object GetFilterValue(SaveableEquipmentData data, FilterType filter)
        {
            object value = filter switch
            {
                FilterType.Level => data.level,
                FilterType.Category => (EquipmentEntityOverview.Instance.GetAsset(data.MasterID) as EquipmentEntityOverview.AssetReferenceMasterID).equipmentType,
                FilterType.Element => (EquipmentEntityOverview.Instance.GetAsset(data.MasterID) as EquipmentEntityOverview.AssetReferenceMasterID).elementType,
                FilterType.DateCreated => data.InstanceData.dateCreatedString,
                FilterType.DateOwned => data.InstanceData.dateOwnedString,
                FilterType.Name => data.MasterID,
                _ => 0,
            };
            return value;
        }

        protected override void InitItem(int index, EquipmentPreviewItem item)
        {
            base.InitItem(index, item);
            item.onLoaded = onItemLoaded;
        }
    }
}
