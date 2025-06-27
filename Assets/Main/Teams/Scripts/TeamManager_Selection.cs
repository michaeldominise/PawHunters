using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class TeamManager_Selection : TeamManager
    {
        [SerializeField] int layerSortingOrder = 5;

        protected override int LayerSortingOrder => layerSortingOrder;

        protected override void Refresh() { }

        public void EntityUnload(EntityMainController entity)
        {
            entity.CharacterData.UnloadAssets(SaveableCharacterData.AssetType.character);
            Despawn(entity);
        }

        public async void CharacterLoad(int index, SaveableCharacterData characterData)
        {
            await characterData.LoadAssets(SaveableCharacterData.AssetType.character);
            Spawn(characterData.GetPrefab(), init: entity => EntityInit(index, entity, characterData));
        }
    }
}
