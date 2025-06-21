using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public enum RarityType
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
        Elite,
        Mythic,
        Legendary
    }

    [System.Serializable]
    public class RarityColorTheme
    {
        public Color common = new(0.8f, 0.8f, 0.8f, 1f);   // Light Gray
        public Color uncommon = new(0.3f, 0.8f, 0.3f, 1f);   // Green
        public Color rare = new(0.2f, 0.4f, 0.9f, 1f);   // Blue
        public Color epic = new(0.6f, 0.2f, 0.8f, 1f);   // Purple
        public Color elite = new(0.9f, 0.4f, 0.2f, 1f);   // Orange-Red
        public Color mythic = new(1.0f, 0.2f, 0.7f, 1f);   // Pink-Magenta
        public Color legendary = new (1.0f, 0.84f, 0.0f, 1f);  // Gold
        public Color Default = new(1.0f, 1.0f, 1.0f, 1f); // White (fallback)
        public Color GetColor(RarityType rarityType)
            => rarityType switch
            {
                RarityType.Common => common,
                RarityType.Uncommon => uncommon,
                RarityType.Rare => rare,
                RarityType.Epic => epic,
                RarityType.Elite => elite,
                RarityType.Mythic => mythic,
                RarityType.Legendary => legendary,
                _ => Default,
            };
    }
}
