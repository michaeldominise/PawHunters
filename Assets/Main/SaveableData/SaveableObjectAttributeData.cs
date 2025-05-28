using UnityEngine;


namespace PawHunters
{
    [System.Serializable]
    public class SaveableObjectAttributeData : SaveableData
    {
        [System.Serializable]
        public class Attribute
        {
            public int health = 100;
            public int attack = 10;
            public int defense = 2;
            public int speed = 3;
            public float critChance = 0.1f;
            public float critDamage = 1.2f;
            public float counterChance = 0.1f;
            public float comboChance = 0.1f;
        }

        public string masterID;
        public int level;
        public Attribute attribute;
    }

    [System.Serializable]
    public class SaveableCharacterData : SaveableObjectAttributeData { }
}
