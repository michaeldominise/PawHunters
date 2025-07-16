using System;
using System.Collections;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class HuntersTab : MonoBehaviour
    {
        [SerializeField] ScrollRectPoolHandler_Hunters scrollRectPoolHandler;

        public ScrollRectPoolHandler_Hunters ScrollRectPoolHandler => scrollRectPoolHandler;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            scrollRectPoolHandler.Init(Bag.Instance.hunterCollection);   
        }

        public void Init(Action<EntityPreviewItem<SaveableCharacterData>> onItemLoaded) => scrollRectPoolHandler.onItemLoaded = onItemLoaded;
        public void Refresh() => scrollRectPoolHandler.SetSortedList();
    }
}
