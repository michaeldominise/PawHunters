using System;
using UnityEngine;

namespace PawHunters
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "GameData/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Serializable]
        public class Attribute
        {
            public float attackDelay = 0.15f;
            public float attackRange = 2f;
        }

        [Serializable]
        public class InGameObjects
        {
            public BulletData bulletData;
        }

        public Attribute attribute;
        public InGameObjects inGameObjects;
    }
}
