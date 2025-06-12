using UnityEngine;

namespace LabHaven.PawHunters
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake() => Instance = (T)this;
    }
}
