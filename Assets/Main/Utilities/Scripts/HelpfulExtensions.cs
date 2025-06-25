using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public static class NumberFormatterExtension
    {
        public static string Format(this float number)
        {
            if (number >= 1_000_000_000_000)
                return (number / 1_000_000_000_000).ToString("0.#") + "T";
            else if (number >= 1_000_000_000_000)
                return (number / 1_000_000_000).ToString("0.#") + "B";
            else if (number >= 1_000_000)
                return (number / 1_000_000).ToString("0.#") + "M";
            else if (number >= 1_000)
                return (number / 1_000).ToString("0.#") + "K";
            else
                return number.ToString("0");
        }
    }

    public static class ByteExtension
    {
        public enum SizeUnits { B, KB, MB, GB, TB, PB, EB, ZB, YB }

        public static string ToSize(this long value, SizeUnits unit)
            => (value / (double)Math.Pow(1024, (long)unit)).ToString("0.00");
    }

    public static class RarityExtension
    {
        public static RarityType LevelToRarity(this int value)
        {
            int maxLength = Enum.GetNames(typeof(RarityType)).Length;
            var maxLevel = 0;
            for (var x = 0; x < maxLength; x++)
            {
                maxLevel += x * 10;
                if (value < maxLevel)
                    return (RarityType)x;
            }
            return RarityType.Legendary;
        }

        public static int RarityLowestLevel(this RarityType rarityType)
        {
            if (rarityType == RarityType.None)
                return 0;

            var index = (int)rarityType;
            var level = 0;
            for (var x = 0; x < index - 1; x++)
                level += x * 10;
            return level + 1;
        }

        public static int RarityHighestLevel(this RarityType rarityType)
        {
            if (rarityType == RarityType.None)
                return 0;

            var index = (int)rarityType;
            var level = 0;
            for (var x = 0; x < index; x++)
                level += x * 10;
            return level - 1;
        }
    }
}