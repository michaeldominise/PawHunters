using System;
using UnityEngine;


namespace PawHunters
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "GameData/BulletData")]
    public class BulletData : ScriptableObject
    {
        [Serializable]
        public class Attribute
        {
            public Vector2 force = Vector2.one;
            public float lifeSpan = 1f;
            public float explosionRadius = 0.25f;
            public int damage = 10;
        }

        [Serializable]
        public class InGameObjects
        {
            public Material bulletMaterial;
            public BulletController bulletPrefab;
        }

        public Attribute attribute;
        public InGameObjects inGameObjects;
    }
}
