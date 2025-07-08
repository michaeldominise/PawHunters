using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class StatsItemManager : Spawner<StatsItem>
    {
        [SerializeField] StatsItem prefab;

        public void Init(IEnumerable<StatsItem.Data> dataList)
        {
            Clear();
            foreach (var data in dataList)
                Spawn(prefab, init: item => item.Init(data));

            SpawnParent.gameObject.SetActive(activeList.Count > 0);
            LayoutRebuilder.ForceRebuildLayoutImmediate(spawnParent.transform as RectTransform);
        }
    }
}
