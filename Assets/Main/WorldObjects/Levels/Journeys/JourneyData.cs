using UnityEngine;

namespace PawHunters
{
    public abstract class JourneyData : ScriptableObject
    {
        public enum Type { Default, Battle, Boss }

        [TextArea]
        public string description;
        public string buttonLabel = "Next";
        public Type type;

        public abstract void Init();
        public abstract void Execute();
    }
}
