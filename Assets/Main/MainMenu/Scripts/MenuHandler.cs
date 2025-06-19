using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class MenuHandler : MonoBehaviour
    {
        [SerializeField] bool canUnselect;
        [SerializeField] List<MenuItem> itemList;
        [SerializeField] MenuItem selectedItem;

        public IEnumerator Start()
        {
            yield return null;
            Init();
        }

        public void Init() => itemList.ForEach(x => x.Init(OnSelect, x == selectedItem));

        public void OnSelect(MenuItem item)
        {
            selectedItem = canUnselect && item == selectedItem ? null : item;
            itemList.ForEach(x => x.SetState(x == selectedItem ? MenuItem.State.Selected : MenuItem.State.NotSelected));
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }
    }
}
