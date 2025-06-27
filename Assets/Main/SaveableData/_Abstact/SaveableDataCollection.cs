using System.Collections.Generic;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [System.Serializable]
    public class Collection<T> : SaveableData where T : SaveableData
    {
        public List<T> items = new();
    }

    [System.Serializable]
    public class TeamCollection : Collection<SaveableTeamData_CharacterInstance>
    {
        [SerializeField] int selectedIndex;

        public int SelectedIndex
        {
            get => selectedIndex;
            set => selectedIndex = (int)Mathf.Repeat(value, items.Count);
        }
        public SaveableTeamData_CharacterInstance SelectedTeamData => items[Mathf.Clamp(selectedIndex, 0, items.Count)];
    }
}
