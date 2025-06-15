using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "GameData/LevelData")]
    public class StageData : ScriptableObject
    {
        [System.Flags]
        public enum AssetType
        {
            none,
            environmentItem = 1 << 0,
            journeys = 1 << 0,
            descriptions = 1 << 0,
            all = (1 << 30) - 1
        }

        public string title;
        public int maxRound = -1;

        public AddressableData data;

        [System.Serializable]
        public class AddressableData : AddressableData<AssetType>
        {
            public AssetReferenceMasterID<EnvironmentItem> environmentItem;
            public ListAssetReferenceMasterID<JourneyData> journeys;
            public AssetReferenceMasterID<TextAsset> battleDescriptions;
            public AssetReferenceMasterID<TextAsset> bossDescriptions;
            public AssetReferenceMasterID<TextAsset> negativeDescriptions;
            public AssetReferenceMasterID<TextAsset> positiveDescriptions;

            public override List<IAssetReferenceMasterID> GetAssetReferences(AssetType assetType)
            {
                var list = new List<IAssetReferenceMasterID>();
                if (assetType.HasFlag(AssetType.environmentItem))
                    list.Add(environmentItem);
                if (assetType.HasFlag(AssetType.journeys))
                    list.AddRange(journeys);
                if (assetType.HasFlag(AssetType.descriptions))
                {
                    list.Add(battleDescriptions);
                    list.Add(bossDescriptions);
                    list.Add(negativeDescriptions);
                    list.Add(positiveDescriptions);
                }

                return list;
            }

            public override async Task LoadAssets(AssetType assetType)
            {
                await base.LoadAssets(assetType);
                if (!assetType.HasFlag(AssetType.journeys))
                    return;

                var loadTask = new List<Task>();
                for (var x = 0; x < journeys.Count; x++)
                    loadTask.Add(journeys[x].Asset.LoadAssets());

                if (loadTask.Count > 0)
                    await Task.WhenAll(loadTask);
            }

            public override void UnloadAssets(AssetType assetType)
            {
                if(assetType.HasFlag(AssetType.journeys))
                    for (var x = 0; x < journeys.Count; x++)
                        journeys[x].Asset?.UnloadAssets();
                base.UnloadAssets(assetType);
            }
        }
    }
}
