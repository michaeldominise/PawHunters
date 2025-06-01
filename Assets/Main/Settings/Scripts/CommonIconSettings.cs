using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "CommonIconSettings", menuName = "GameData/Settings/CommonIconSettings")]
    public class CommonIconSettings : ScriptableObject
    {
        public enum Icon
        {
            None,
            Shield,
            Speed,
            Health,
            Attack,
        }

        public Sprite shield;
        public Sprite speed;
        public Sprite health;
        public Sprite attack;

        public Sprite GetIcon(Icon icon)
            => icon switch
            {
                Icon.Shield => shield,
                Icon.Speed => speed,
                Icon.Health => health,
                Icon.Attack => attack,
                _ => null
            };
    }
}
