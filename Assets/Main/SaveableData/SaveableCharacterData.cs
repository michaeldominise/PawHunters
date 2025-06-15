using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LabHaven.PawHunters
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
            public float specialSkillMax;
        }

        public string masterID;
        public int level;
        public Attribute attribute;
    }

    [System.Serializable]
    public class SaveableCharacterData : SaveableObjectAttributeData
    {
        public AssetReferenceMasterID<EntityMainController> PrefabAssetReference => EntityOverview.Instance.GetAsset(masterID);
        public EntityMainController GetPrefab() => PrefabAssetReference.Asset;

        [SerializeField] List<string> skillDataMasterIdList;
        IEnumerable<AssetReferenceMasterID<SkillData>> SkillDataAssetReferenceList => skillDataMasterIdList.Select(x => SkillDataOverview.Instance.GetAsset(x));
        public IEnumerable<SkillData> SkillDataList => skillDataMasterIdList.Select(x => SkillDataOverview.Instance.GetAsset(x).Asset);

        public override List<IAssetReferenceMasterID> GetAssetReference()
        {
            var list = new List<IAssetReferenceMasterID>();
            if (PrefabAssetReference != null)
                list.Add(PrefabAssetReference);
            list.AddRange(SkillDataAssetReferenceList);
            return list;
        }
    }
}
