using System.Collections.Generic;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [System.Serializable]
    public partial class UserData : SaveableData
    {
        public static UserData Instance => AppManager.Instance.userData;

        public BattleData battleData;
        public TeamCollection teamCollection = new() { items = new() { new("Main Pact") } };
        public Bag bag;
    }
}
