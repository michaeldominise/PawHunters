using UnityEngine;

namespace PawHunters
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
    }
}
