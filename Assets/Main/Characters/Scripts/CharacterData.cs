using UnityEngine;


namespace PawHunters
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "GameData/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        [System.Serializable]
        public class Attribute
        {
            public int maxHealth = 100;
            public float movementSpeed = 1f;
            public float randomPathRange = 5.0f;
            public float rotateSpeed = 1f;
            public Vector2 attackCooldown = new Vector2(0.5f, 3);

            [Range(0, 1)]
            public float aggressiveValue = 0.5f;
        }

        [System.Serializable]
        public class InGameObjects
        {
            public WeaponData weaponData;
            public EntityMainController playerPrefab;
        }

        public Attribute attribute;
        public InGameObjects inGameObjects;
    }
}
