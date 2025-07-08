using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class SkillsPreview : Spawner<SkillSelectionItem>
    {
        [SerializeField] SkillSelectionItem prefab;

        public void Init(IEnumerable<SkillData> skillDataList)
        {
            Clear();

            foreach (var skillData in skillDataList)
                Spawn(prefab, init: item => item.Init(skillData, null));

            SpawnParent.gameObject.SetActive(activeList.Count > 0);
            LayoutRebuilder.ForceRebuildLayoutImmediate(spawnParent.transform as RectTransform);
        }
    }
}
