using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class CopyRectSizeToLocalScale : MonoBehaviour
    {
        [SerializeField] RectTransform target;
        [SerializeField] float refreshInterval = 5;

        IEnumerator Start()
        {
            while(true)
            {
                Refresh();
                yield return new WaitForSeconds(refreshInterval);
            }
        }

        [Button]
        private void Refresh()
        {
            transform.localScale = new Vector3(target.rect.size.x, target.rect.size.y, 1);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
