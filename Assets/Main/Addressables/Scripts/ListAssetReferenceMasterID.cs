using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [System.Serializable]
    [InlineProperty]
    public class ListAssetReferenceMasterID<T> : IEnumerable<AssetReferenceMasterID<T>>, IEnumerable<IAssetReferenceMasterID> where T : Object
    {
        [SerializeField, HideInInspector] List<AssetReferenceMasterID<T>> dataList;

#if UNITY_EDITOR
        [SerializeField, HideInInspector] T[] assetList;
        [ShowInInspector, HideLabel]
        T[] AssetList
        {
            get => assetList;
            set
            {
                assetList = value;
                dataList = assetList.Select(x => new AssetReferenceMasterID<T>(x)).ToList();
            }
        }
#endif

        public IEnumerator<IAssetReferenceMasterID> GetEnumerator() => dataList.GetEnumerator();
        IEnumerator<AssetReferenceMasterID<T>> IEnumerable<AssetReferenceMasterID<T>>.GetEnumerator() => dataList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => dataList.GetEnumerator();

        public void Refresh()
        {
#if UNITY_EDITOR
            dataList = assetList.Select(x => new AssetReferenceMasterID<T>(x)).ToList();
#endif
        }

        public AssetReferenceMasterID<T> this[int key] => dataList[key];
        public int Count => dataList.Count;
    }
}
