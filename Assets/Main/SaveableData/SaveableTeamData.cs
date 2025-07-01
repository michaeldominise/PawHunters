using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public abstract class SaveableTeamData : SaveableData<SaveableTeamData.AssetType>
    {
        [Flags]
        public enum AssetType
        {
            None,
            CharacterPrefab = 1 << 0,
            CharacterSkills = 1 << 1,
            EquipmentPrefab = 1 << 2,
            EquipmentSkills = 1 << 3,

            AllPrefabs = CharacterPrefab | EquipmentPrefab,
            AllSkills = CharacterSkills | EquipmentSkills,

            All = (1 << 30) - 1
        }

        public string teamName = "Sub Pact";
        public abstract List<SaveableCharacterData> Characters { get; }
        public abstract List<SaveableEquipmentData> Equipments { get; }

        public override List<IAssetReferenceMasterID> GetAssetReference(AssetType assetType)
        {
            var list = new List<IAssetReferenceMasterID>();
            var characterAssetType = SaveableDataEntity.AssetType.None;
            if (assetType.HasFlag(AssetType.CharacterPrefab))
                characterAssetType |= SaveableDataEntity.AssetType.Prefab;
            if (assetType.HasFlag(AssetType.CharacterSkills))
                characterAssetType |= SaveableDataEntity.AssetType.Skills;

            var equipmentAssetType = SaveableDataEntity.AssetType.None;
            if (assetType.HasFlag(AssetType.EquipmentPrefab))
                equipmentAssetType |= SaveableDataEntity.AssetType.Prefab;
            if (assetType.HasFlag(AssetType.EquipmentSkills))
                equipmentAssetType |= SaveableDataEntity.AssetType.Skills;

            Characters.Where(x => x != null).ToList().ForEach(x => list.AddRange(x.GetAssetReference(characterAssetType)));
            Equipments.Where(x => x != null).ToList().ForEach(x => list.AddRange(x.GetAssetReference(equipmentAssetType)));
            return list;
        }

        public SaveableTeamData() { }
        public SaveableTeamData(string teamName) => this.teamName = teamName;
    }
}
