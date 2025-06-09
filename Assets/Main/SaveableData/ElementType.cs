using UnityEngine;

namespace LabHaven.PawHunters
{
    public enum ElementType
    {
        None,
        Normal = 1 << 0,
        Water = 1 << 1,
        Fire = 1 << 2,
        Electric = 1 << 3,
        Dark = 1 << 4,
        Fighter = 1 << 5,
        Bug = 1 << 6,
        Dragon = 1 << 7,
        Fairy = 1 << 8,
        Flying = 1 << 9,
        Ghost = 1 << 10,
        Grass = 1 << 11,
        Ground = 1 << 12,
        Ice = 1 << 13,
        Poison = 1 << 14,
        Psychic = 1 << 15,
        Rock = 1 << 16,
        Steel = 1 << 17,
        All = 1 << 32 - 1,
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
