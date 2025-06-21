using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace LabHavenInteractive.PawHunters
{
    public class TeamManager_Selection : TeamManager
    {
        [SerializeField] int layerSortingOrder = 5;
        protected override int LayerSortingOrder => layerSortingOrder;
    }
}
