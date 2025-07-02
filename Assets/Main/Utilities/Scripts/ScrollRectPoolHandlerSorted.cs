using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class ScrollRectPoolHandlerSorted<T1, T2, TFilterType> : ScrollRectPoolHandler<T1, T2>
        where T1 : PoolItem<T2>
        where T2 : SaveableData
        where TFilterType : Enum
    {
        public enum OrderType { Acending, Decending }

        [SerializeField] protected TFilterType filterType;

        protected abstract List<KeyValuePair<TFilterType, OrderType>> DefaultFilters { get; }

        public override void SetSortedList()
        {
            var sortedList = DefaultFilters.FirstOrDefault(x => x.Key.Equals(filterType)).Value == OrderType.Acending
                ? data.items.OrderBy(x => GetFilterValue(x, filterType))
                : data.items.OrderByDescending(x => GetFilterValue(x, filterType));

            foreach (var defaultFilter in DefaultFilters)
            {
                if (defaultFilter.Key.Equals(filterType))
                    continue;
                sortedList = defaultFilter.Value == OrderType.Acending
                    ? sortedList.ThenBy(x => GetFilterValue(x, filterType))
                    : sortedList.ThenByDescending(x => GetFilterValue(x, filterType));
            }
            SetSortedList(sortedList.ToList());
        }

        [Button]
        public void SetSortedList(TFilterType filterType)
        {
            this.filterType = filterType;
            SetSortedList();
        }

        public abstract object GetFilterValue(T2 data, TFilterType filter);
    }
}
