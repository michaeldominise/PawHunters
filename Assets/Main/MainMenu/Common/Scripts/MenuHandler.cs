using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class MenuHandler : MonoBehaviour
    {
        [SerializeField] BackNavigationHandler.BackHandlerMode backHandlerMode = BackNavigationHandler.BackHandlerMode.Execute;
        [SerializeField] bool canUnselect;
        [SerializeField] List<MenuItem> itemList;
        [SerializeField] MenuItem defaultSelectedItem;
        [ShowInInspector, ReadOnly] MenuItem CurrentSelectedItem { get; set; }

        public int SelectedIndex => CurrentSelectedItem == null ? -1 : CurrentSelectedItem.Index;

        public IEnumerator Start()
        {
            yield return null;
            Init();
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        public void Init()
        {
            for(var x = 0; x < itemList.Count; x++)
                itemList[x].Init(OnSelect, itemList[x] == defaultSelectedItem, x);
        }

        public void OnSelect(MenuItem item) => OnSelect(item, backHandlerMode);
        void OnSelect(MenuItem item, BackNavigationHandler.BackHandlerMode backHandlerMode)
        {
            if (item == CurrentSelectedItem)
            {
                if (canUnselect)
                    item = null;
                else
                    return;
            }

            var lastSelectedItem = CurrentSelectedItem;
            CurrentSelectedItem = item;
            itemList.ForEach(x => x.SetState(x == CurrentSelectedItem ? MenuItem.State.Selected : MenuItem.State.NotSelected));
            BackNavigationHandler.Add(this, () =>
            {
                var selectedItem = backHandlerMode == BackNavigationHandler.BackHandlerMode.Execute ? lastSelectedItem : defaultSelectedItem;
                OnSelect(selectedItem, BackNavigationHandler.BackHandlerMode.DoNothing);
            }, backHandlerMode);
        }
    }
}
