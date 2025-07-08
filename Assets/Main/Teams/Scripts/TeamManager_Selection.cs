using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class TeamManager_Selection : TeamManager
    {
        [SerializeField] int layerSortingOrder = 5;
        [SerializeField] TMP_InputField teamNameInput;
        [SerializeField] Button prevTeamButton;
        [SerializeField] Button nextTeamButton;
        [SerializeField] SaveableTeamData_CharacterInstance teamDataInstance;
        [SerializeField] EquipmentSlotManager equipmentSlotManager;

        public SaveableTeamData_CharacterInstance TeamDataInstance => teamDataInstance;
        public EquipmentSlotManager EquipmentSlotManager => equipmentSlotManager;
        protected override int LayerSortingOrder => layerSortingOrder;
        TeamCollection teamCollection;
        Action onUpdateDetails;

        private void Start()
        {
            teamNameInput.onEndEdit.AddListener(TextNameInput_OnUpdate);
            prevTeamButton.onClick.AddListener(PreviousTeam);
            nextTeamButton.onClick.AddListener(NextTeam);
        }

        public void Init(TeamCollection teamCollection, Action onUpdateDetails)
        {
            this.teamCollection = teamCollection;
            this.onUpdateDetails = onUpdateDetails;

            ReselectTeam();
        }

        protected override void Refresh() { }

        public void EntityUnload(EntityMainController entity)
        {
            entity.Data.UnloadAssets(SaveableDataEntity.AssetType.Prefab);
            Despawn(entity);
        }

        public async void CharacterLoad(int index, SaveableCharacterData characterData)
        {
            await characterData.LoadAssets(SaveableDataEntity.AssetType.Prefab);
            Spawn(characterData.GetPrefab(), init: entity => EntityInit(index, entity, characterData));
        }

        public void ReselectTeam() => SetTeamIndex(teamCollection.SelectedIndex);
        public void NextTeam() => SetTeamIndex(teamCollection.SelectedIndex + 1);
        public void PreviousTeam() => SetTeamIndex(teamCollection.SelectedIndex - 1);
        public void SetTeamIndex(int index)
        {
            teamCollection.SelectedIndex = index;
            teamDataInstance = SaveableData.Initialize(teamDataInstance, teamCollection.SelectedTeamData, OnValueChange);
            UpdateDetails();
        }

        public async void UpdateDetails()
        {
            teamNameInput.text = TeamDataInstance.teamName;
            await equipmentSlotManager.Init(teamDataInstance);
            await Init(teamDataInstance);
            onUpdateDetails?.Invoke();
        }

        private void TextNameInput_OnUpdate(string teamName)
        {
            TeamDataInstance.teamName = teamName;
            TeamDataInstance.SetDirty();
        }

        void OnValueChange()
        {

        }
    }
}
