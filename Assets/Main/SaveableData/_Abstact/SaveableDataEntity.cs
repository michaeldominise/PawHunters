using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class SaveableDataEntity : SaveableData<SaveableDataEntity.AssetType>, IAttribute
    {
        [Flags]
        public enum AssetType
        {
            None,
            Prefab = 1 << 0,
            Skills = 1 << 1,
            All = (1 << 30) - 1
        }

        [SerializeField] protected string masterId;
        public string MasterID => masterId;

        public int level;

        [SerializeField] Attribute attribute;
        [SerializeField] List<string> skillDataMasterIdList;

        public RarityType Rarity => level.LevelToRarity();
        public Attribute Attribute => attribute;

        public abstract IAssetReferenceMasterID PrefabAssetReference { get; }
        public EntityMainController GetPrefab() => PrefabAssetReference.Asset as EntityMainController;

        IEnumerable<IAssetReferenceMasterID<SkillData>> SkillDataAssetReferenceList => skillDataMasterIdList.Select(x => SkillDataOverview.Instance.GetAsset(x));
        public IEnumerable<SkillData> SkillDataList => skillDataMasterIdList.Select(x => SkillDataOverview.Instance.GetAsset(x).Asset as SkillData).Where(x => x != null);

        public SaveableDataEntity()
        {
            try
            {
                level = UnityEngine.Random.Range(0, Enum.GetValues(typeof(RarityType)).Length);
            }
            catch { }
        }

        public override List<IAssetReferenceMasterID> GetAssetReference(AssetType assetType)
        {
            var list = new List<IAssetReferenceMasterID>();
            if (assetType.HasFlag(AssetType.Prefab))
                list.Add(PrefabAssetReference );
            if (assetType.HasFlag(AssetType.Skills))
                list.AddRange(SkillDataAssetReferenceList);
            return list;
        }
    }
}
