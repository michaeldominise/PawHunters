using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace LabHaven.PawHunters
{
    public class TeamManager_Selection : TeamManager
    {
        [SerializeField] int layerSortingOrder = 5;
        protected override int LayerSortingOrder => layerSortingOrder;
    }
}
