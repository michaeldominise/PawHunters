using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableCharacterData : SaveableData<SaveableCharacterData.AssetType>
    {
        [Flags]
        public enum AssetType
        {
            none,
            character = 1 << 0,
            skills = 1 << 0,
            all = (1 << 30) - 1
        }

        [Serializable]
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

        [SerializeField] List<string> skillDataMasterIdList;

        public AssetReferenceMasterID<EntityMainController> PrefabAssetReference => EntityOverview.Instance.GetAsset(masterID);
        public EntityMainController GetPrefab() => PrefabAssetReference.Asset;

        IEnumerable<AssetReferenceMasterID<SkillData>> SkillDataAssetReferenceList => skillDataMasterIdList.Select(x => SkillDataOverview.Instance.GetAsset(x));
        public IEnumerable<SkillData> SkillDataList => skillDataMasterIdList.Select(x => SkillDataOverview.Instance.GetAsset(x).Asset).Where(x => x != null);

        public override List<IAssetReferenceMasterID> GetAssetReference(AssetType assetType)
        {
            var list = new List<IAssetReferenceMasterID>();
            if(assetType.HasFlag(AssetType.character))
                list.Add(PrefabAssetReference);
            if(assetType.HasFlag(AssetType.skills))
                list.AddRange(SkillDataAssetReferenceList);
            return list;
        }
    }
}
