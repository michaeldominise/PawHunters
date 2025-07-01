using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace LabHavenInteractive.PawHunters
{
    /// <summary>
    /// Used to order sprite layers (monster parts).
    /// </summary>
    public class LayerManager : MonoBehaviour
    {
        public SortingGroup SortingGroup;
        public List<SpriteRenderer> Sprites;

        public void SetSortingGroupOrder(int index) => SortingGroup.sortingOrder = index;

        [Button]
        public void GetSpritesBySortingOrder() => Sprites = GetComponentsInChildren<SpriteRenderer>(true).OrderBy(i => i.sortingOrder).ToList();

        [Button]
        public void SetSpritesBySortingOrder()
        {
            for (var i = 0; i < Sprites.Count; i++)
                Sprites[i].sortingOrder = 5 * i;
        }

        public void SetSpritesMaskInteraction(SpriteMaskInteraction maskInteraction) => Sprites.ForEach(x => x.maskInteraction = maskInteraction);
    }
}