using UnityEngine;

namespace PawHunters
{
    public static class NumberFormatter
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
}
