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
        public enum SizeUnits
        {
            Byte, KB, MB, GB, TB, PB, EB, ZB, YB
        }

        public static string ToSize(this long value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (long)unit)).ToString("0.00");
        }
    }
}