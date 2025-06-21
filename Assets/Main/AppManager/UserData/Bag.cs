using System.Collections.Generic;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [System.Serializable]
    public class Bag : SaveableData
    {
        public static Bag Instance => UserData.Instance.bag;

        public Collection<SaveableCharacterData> hunterCollection;
    }
}
