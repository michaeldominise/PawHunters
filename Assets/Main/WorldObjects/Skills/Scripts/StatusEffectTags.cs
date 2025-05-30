using UnityEngine;

namespace PawHunters
{
    [System.Flags]
    public enum StatusEffectTags
    {
        None,

        //Action Tags
        BasicAttack = 1 << 0,
        SpecialSkill = 1 << 1,
        Heal = 1 << 2,
        Damage = 1 << 3,
        Skill = 1 << 4,
    }
}
