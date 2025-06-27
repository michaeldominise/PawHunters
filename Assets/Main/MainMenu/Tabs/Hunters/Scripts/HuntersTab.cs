using System;
using System.Collections;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class HuntersTab : MonoBehaviour
    {
        [SerializeField] ScrollRectPoolHandler_Hunters scrollRectPoolHandler_Hunters;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            scrollRectPoolHandler_Hunters.Init(Bag.Instance.hunterCollection);   
        }

        public void Init(Action<HunterPreviewItem> onItemLoaded) => scrollRectPoolHandler_Hunters.onItemLoaded = onItemLoaded;
        public void Refresh() => scrollRectPoolHandler_Hunters.SetSortedList();
    }
}
