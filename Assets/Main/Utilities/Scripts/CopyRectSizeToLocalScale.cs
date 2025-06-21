using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class CopyRectSizeToLocalScale : MonoBehaviour
    {
        [SerializeField] RectTransform target;

        [Button]
        private void OnPostRender()
        {
            transform.localScale = new Vector3(target.rect.size.x, target.rect.size.y, 1);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
