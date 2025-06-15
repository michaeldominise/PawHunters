using UnityEngine;

namespace LabHaven.PawHunters
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if(Instance)
            {
                gameObject.SetActive(false);
                return;
            }

            Instance = (T)this;
        }
    }
}
