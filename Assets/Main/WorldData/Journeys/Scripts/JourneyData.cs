using UnityEngine;

namespace PawHunters
{
    public abstract class JourneyData : ScriptableObject
    {
        [TextArea]
        public string description;
        public string buttonLabel = "Next";

        public abstract void Init();
        public abstract void Execute();
    }
}
