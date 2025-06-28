using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LabHavenInteractive.PawHunters
{
    [Serializable]
    public class SaveableEquipmentData : SaveableData<SaveableEquipmentData.AssetType>
    {
        [Flags]
        public enum AssetType
        {
            None,
            Prefab = 1 << 0,
            Skills = 1 << 1,
            All = (1 << 30) - 1
        }

        [Flags]
        public enum CategoryType
        {
            none,
            weapon,
        }

        [SerializeField] string masterId = "Character.BlackCat";
        public string MasterID => masterId;

        public int level;
        public Attribute attribute;

        [SerializeField] List<string> skillDataMasterIdList;

        public RarityType Rarity => level.LevelToRarity();
        public AssetReferenceMasterID<EntityMainController> PrefabAssetReference => EntityOverview.Instance.GetAsset(MasterID);
        public EntityMainController GetPrefab() => PrefabAssetReference.Asset;

        IEnumerable<AssetReferenceMasterID<SkillData>> SkillDataAssetReferenceList => skillDataMasterIdList.Select(x => SkillDataOverview.Instance.GetAsset(x));
        public IEnumerable<SkillData> SkillDataList => skillDataMasterIdList.Select(x => SkillDataOverview.Instance.GetAsset(x).Asset).Where(x => x != null);

        public override List<IAssetReferenceMasterID> GetAssetReference(AssetType assetType)
        {
            var list = new List<IAssetReferenceMasterID>();
            if(assetType.HasFlag(AssetType.Prefab))
                list.Add(PrefabAssetReference);
            if(assetType.HasFlag(AssetType.Skills))
                list.AddRange(SkillDataAssetReferenceList);
            return list;
        }
    }
}
