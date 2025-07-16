using System;
using System.Collections;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class EquipmentsTab : MonoBehaviour
    {
        [SerializeField] ScrollRectPoolHandler_Equipments scrollRectPoolHandler;

        public ScrollRectPoolHandler_Equipments ScrollRectPoolHandler => scrollRectPoolHandler;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            scrollRectPoolHandler.Init(Bag.Instance.equipmentCollection);   
        }

        public void Init(Action<EntityPreviewItem<SaveableEquipmentData>> onItemLoaded) => scrollRectPoolHandler.onItemLoaded = onItemLoaded;
        public void Refresh() => scrollRectPoolHandler.SetSortedList();
    }
}
