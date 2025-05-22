using UnityEngine;


namespace PawHunters
{
    [System.Serializable]
    public class SaveableCharacterData : SaveableData
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

        public string masterID;
        public Attribute attribute;
    }
}
