using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public enum ElementType
    {
        None, // No element assigned

        Normal = 1 << 0,  // A well-rounded type with no glaring strengths or weaknesses.
        Water = 1 << 1,  // Balanced stats across the board, making it adaptable in any situation.
        Fire = 1 << 2,  // High speed and attack with a fiery crit rate — burns fast, but not built for endurance.
        Electric = 1 << 3,  // Fast and zappy, with quick attacks and decent crit potential, but fragile defenses.
        Dark = 1 << 4,  // Cunning and precise, with strong crits and a sneaky balance of power.
        Fighter = 1 << 5,  // Hits hard and endures well — the brawler’s choice with high attack and health.
        Bug = 1 << 6,  // Fast but fragile — strikes quickly, then scuttles back before getting crushed.
        Dragon = 1 << 7,  // Powerhouse with top-tier stats in nearly every category — fear the dragon's wrath.
        Fairy = 1 << 8,  // Charming but dangerous — solid defenses with a deceptively high crit rate.
        Flying = 1 << 9,  // Swift and evasive — glides in with good speed and decent offensive pressure.
        Ghost = 1 << 10, // Elusive and deadly — moderate stats but a high chance of devastating crits.
        Grass = 1 << 11, // Resilient and steady — focused on healing and outlasting opponents.
        Ground = 1 << 12, // Tanky and powerful — can take hits and return them with raw force.
        Ice = 1 << 13, // Glass cannon — low defense but high attack and strong crit bursts.
        Poison = 1 << 14, // A tricky type with quick strikes and a sting in every blow.
        Psychic = 1 << 15, // Mind over muscle — high crit chance and good speed for tactical strikes.
        Rock = 1 << 16, // Solid as stone — massive defense and health, but slow to act.
        Steel = 1 << 17, // An unbreakable wall — supreme defense and survivability with steady output.

        All = 1 << 32 - 1, // Represents all types — used for universal effects or targeting
    }

    [System.Serializable]
    public class ElementColorTheme
    {
        public Color Normal = new(0.8f, 0.8f, 0.8f, 1f); // Light gray
        public Color Water = new(0.2f, 0.4f, 0.9f, 1f); // Deep blue
        public Color Fire = new(1.0f, 0.3f, 0.0f, 1f); // Bright red-orange
        public Color Electric = new(1.0f, 1.0f, 0.2f, 1f); // Bright yellow
        public Color Dark = new(0.2f, 0.2f, 0.3f, 1f); // Nearly black/navy
        public Color Fighter = new(0.8f, 0.1f, 0.1f, 1f); // Blood red
        public Color Bug = new(0.5f, 0.7f, 0.2f, 1f); // Olive green
        public Color Dragon = new(0.4f, 0.2f, 0.9f, 1f); // Purple-blue
        public Color Fairy = new(1.0f, 0.7f, 0.9f, 1f); // Soft pink
        public Color Flying = new(0.6f, 0.8f, 1.0f, 1f); // Sky blue
        public Color Ghost = new(0.4f, 0.3f, 0.6f, 1f); // Dusky purple
        public Color Grass = new(0.2f, 0.8f, 0.2f, 1f); // Bright green
        public Color Ground = new(0.7f, 0.5f, 0.2f, 1f); // Earth brown
        public Color Ice = new(0.6f, 0.9f, 1.0f, 1f); // Icy blue
        public Color Poison = new(0.6f, 0.2f, 0.6f, 1f); // Purple
        public Color Psychic = new(1.0f, 0.2f, 0.6f, 1f); // Vivid pink
        public Color Rock = new(0.6f, 0.5f, 0.3f, 1f); // Stone gray
        public Color Steel = new(0.7f, 0.7f, 0.8f, 1f); // Cool metal gray
        public Color Default = new(1.0f, 1.0f, 1.0f, 1f); // White (fallback)

        public Color GetColor(ElementType elementType)
            => elementType switch
            {
                ElementType.Normal => Normal,
                ElementType.Water => Water,
                ElementType.Fire => Fire,
                ElementType.Electric => Electric,
                ElementType.Dark => Dark,
                ElementType.Fighter => Fighter,
                ElementType.Bug => Bug,
                ElementType.Dragon => Dragon,
                ElementType.Fairy => Fairy,
                ElementType.Flying => Flying,
                ElementType.Ghost => Ghost,
                ElementType.Grass => Grass,
                ElementType.Ground => Ground,
                ElementType.Ice => Ice,
                ElementType.Poison => Poison,
                ElementType.Psychic => Psychic,
                ElementType.Rock => Rock,
                ElementType.Steel => Steel,
                _ => Default,
            };
    }
}
