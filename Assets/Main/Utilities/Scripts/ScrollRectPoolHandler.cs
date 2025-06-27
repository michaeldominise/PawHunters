using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class ScrollRectPoolHandler<T1, T2> : Spawner<T1> where T1 : PoolItem<T2> where T2 : SaveableData
    {
        [SerializeField] T1 prefab;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] LayoutGroup layoutGroup;
        [SerializeField] RectTransform filler;
        [SerializeField] float minDistanceToLoad = 1500;
        [SerializeField] int intialItemCount = 25;
        [SerializeField] bool isEndless;
        [ShowInInspector, ReadOnly] int CurrentReloadCount { get; set; }
        [ShowInInspector, ReadOnly] int AdditionalItemCount { get; set; }
        [ShowInInspector, ReadOnly] int MaxItemLimit => data.items.Count;
        [ShowInInspector, ReadOnly] int TotalItemCount => spawnedList.Count + AdditionalItemCount;
        [ShowInInspector, ReadOnly] protected List<T2> SortedDataList { get; set; }
        [ShowInInspector, ReadOnly] protected Collection<T2> data = new();

        RectTransform TargetRect => prefab.transform as RectTransform;
        HorizontalLayoutGroup HorizontalLayoutGroup => layoutGroup as HorizontalLayoutGroup;
        VerticalLayoutGroup VerticalLayoutGroup => layoutGroup as VerticalLayoutGroup;
        GridLayoutGroup GridLayoutGroup => layoutGroup as GridLayoutGroup;
        Vector3 lastContentPosition;
        List<GameObject> emptySlots = new();

        float LayoutGroupXSpacing
        {
            get
            {
                if (HorizontalLayoutGroup)
                    return HorizontalLayoutGroup.spacing;
                else if (GridLayoutGroup)
                    return GridLayoutGroup.spacing.x;
                return 0;
            }
        }

        float LayoutGroupYSpacing
        {
            get
            {
                if (VerticalLayoutGroup)
                    return VerticalLayoutGroup.spacing;
                else if (GridLayoutGroup)
                    return GridLayoutGroup.spacing.y;
                return 0;
            }
        }

        public int GetColumnCount()
        {
            if (HorizontalLayoutGroup)
                return HorizontalLayoutGroup.transform.childCount;
            else if (VerticalLayoutGroup)
                return 1;

            switch (GridLayoutGroup.constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    return GridLayoutGroup.constraintCount;
                case GridLayoutGroup.Constraint.FixedRowCount:
                    var childCount = GridLayoutGroup.transform.childCount;
                    var rows = GridLayoutGroup.constraintCount;
                    return Mathf.CeilToInt((float)childCount / rows);
                case GridLayoutGroup.Constraint.Flexible:
                    var width = ((RectTransform)GridLayoutGroup.transform).rect.width;
                    return Mathf.FloorToInt((width + GridLayoutGroup.spacing.x) / (GridLayoutGroup.cellSize.x + GridLayoutGroup.spacing.x));
            }

            return 1;
        }

        public int GetRowCount()
        {
            if (HorizontalLayoutGroup)
                return HorizontalLayoutGroup.transform.childCount;
            else if (VerticalLayoutGroup)
                return 1;

            switch (GridLayoutGroup.constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    var childCount = GridLayoutGroup.transform.childCount;
                    var column = GridLayoutGroup.constraintCount;
                    return Mathf.CeilToInt((float)childCount / column);
                case GridLayoutGroup.Constraint.FixedRowCount:
                    return GridLayoutGroup.constraintCount;
                case GridLayoutGroup.Constraint.Flexible:
                    var height = ((RectTransform)GridLayoutGroup.transform).rect.height;
                    return Mathf.FloorToInt((height + GridLayoutGroup.spacing.y) / (GridLayoutGroup.cellSize.y + GridLayoutGroup.spacing.y));
            }

            return 1;
        }

        private void Awake()
        {
            lastContentPosition = scrollRect.content.localPosition;
            Preload();
        }

        public void OnEnable()
        {
            scrollRect.onValueChanged.AddListener(OnScrollRectValueChange);
            ReclculateLayout();
            Refresh();
        }

        public void OnDisable() => scrollRect.onValueChanged.RemoveListener(OnScrollRectValueChange);


        void Preload()
        {
            for (var x = 0; x < intialItemCount; x++)
                Spawn(prefab, init: item => InitItem(x, item));
            Clear();
        }

        protected virtual void InitItem(int index, T1 item) => item.name = $"{prefab.name} ({index})";

        void ReclculateLayout()
        {
            var columnCount = GetColumnCount();
            for (var x = 0; x < columnCount || x < emptySlots.Count; x++)
            {
                if (emptySlots.Count > x)
                    continue;

                var rectObj = new GameObject($"EmptySlot{x}", typeof(RectTransform));
                rectObj.transform.SetParent(spawnParent);
                rectObj.transform.SetSiblingIndex(x);
                rectObj.SetActive(false);
                emptySlots.Add(rectObj);
            }
        }

        public void RefreshInit() => Init(data);
        public void Init(Collection<T2> data)
        {
            this.data = SaveableData.Initialize(ref this.data, data, RefreshInit);
            SetSortedList();
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        }

        public virtual void SetSortedList() => SetSortedList(data.items);
        public virtual void SetSortedList(List<T2> dataList)
        {
            SortedDataList = dataList;
            Clear();
            for (var x = 0; x < intialItemCount; x++)
                Spawn(prefab, init: item =>
                {
                    if (x < MaxItemLimit)
                    {
                        item.Init(AdditionalItemCount + x, SortedDataList[(AdditionalItemCount + x) % MaxItemLimit]);
                        item.transform.SetSiblingIndex(emptySlots.Count + x);
                    }
                    else
                        item.gameObject.SetActive(false);
                });
            Refresh();
        }

        private void OnScrollRectValueChange(Vector2 value) => Refresh();

        public void Refresh()
        {
            if (data.items.Count == 0)
                return;

            var deltaValue = scrollRect.content.localPosition - lastContentPosition;
            lastContentPosition = scrollRect.content.localPosition;
            if (scrollRect.vertical && deltaValue.y != 0)
                RefreshLayout(true);
            else if (scrollRect.horizontal && deltaValue.x != 0)
                RefreshLayout(false);
        }

        void RefreshLayout (bool isVertical)
        {
            var contentPos = isVertical ? scrollRect.content.anchoredPosition.y : scrollRect.content.anchoredPosition.x;
            var cellGroupSize = isVertical ? (TargetRect.sizeDelta.y + LayoutGroupYSpacing) : (TargetRect.sizeDelta.x + LayoutGroupXSpacing);
            var reloadCount = Mathf.FloorToInt(isVertical
                ? (contentPos - minDistanceToLoad) / cellGroupSize
                : -(contentPos + minDistanceToLoad) / cellGroupSize);
            var reloadCountDelta = reloadCount - CurrentReloadCount;
            var isNext = reloadCountDelta > 0;

            if (reloadCountDelta == 0 || (isVertical ? contentPos : -contentPos) < minDistanceToLoad || (!isEndless && isNext && TotalItemCount >= MaxItemLimit))
                return;

            CurrentReloadCount = reloadCount;
            var columnRowCount = isVertical ? GetColumnCount() : GetRowCount();
            for (var y = 0; y < Mathf.Abs(reloadCountDelta); y++)
            {
                var childIndex = isNext ? 0 : activeList.Count - 1;
                var remainder = isNext ? Mathf.Min(MaxItemLimit - TotalItemCount, columnRowCount) : (AdditionalItemCount % columnRowCount);
                var itemCount = remainder == 0 || isEndless ? columnRowCount : remainder;

                for (int x = 0; x < itemCount; x++)
                {
                    AdditionalItemCount += isNext ? 1 : -1;

                    var dataIndex = isNext ? (TotalItemCount - 1) % MaxItemLimit : AdditionalItemCount;
                    var item = activeList[childIndex];
                    item.Init(dataIndex, SortedDataList[dataIndex]);
                    activeList.RemoveAt(childIndex);
                    if (isNext)
                    {
                        activeList.Add(item);
                        item.transform.SetAsLastSibling();
                    }
                    else
                    {
                        activeList.Insert(0, item);
                        item.transform.SetSiblingIndex(emptySlots.Count);
                    }

                    if (columnRowCount > itemCount)
                        emptySlots[x].SetActive(isNext);

                    if (TotalItemCount % columnRowCount == (isNext ? 1 : 0))
                    {
                        var fillerSize = filler.sizeDelta;
                        var sizeChange = isVertical ? (cellGroupSize * Vector2.up) : (cellGroupSize * Vector2.right);
                        filler.sizeDelta = (isNext ? sizeChange : -sizeChange) + fillerSize;
                    }
                }
            }
        }

        private void Reset()
        {
            scrollRect = GetComponent<ScrollRect>();
        }
    }
}
