using System;
using System.Threading.Tasks;
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
        public EntityParent SelectedCharacterParent { get; set; }
        public EntityParent SelectedEntityParent { get; set; }
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
            SelectedCharacterParent = null;
            SelectedEntityParent = null;

            ReselectTeam();
        }

        protected override void Refresh() { }

        private void EntityParent_OnStateChange(EntityParent_Selection entityParent)
        {
            if (entityParent.CurrentState.Value == EntityParent_Selection.State.NotSelected && entityParent != SelectedCharacterParent)
                return;

            switch (entityParent.CurrentState.Value)
            {
                case EntityParent_Selection.State.NotSelected:
                    SelectedCharacterParent = null;
                    break;
                case EntityParent_Selection.State.Selected:
                    SelectedCharacterParent = entityParent;
                    break;
                case EntityParent_Selection.State.RemoveClicked:
                    (teamData as SaveableTeamData_CharacterInstance).characterInstanceList[entityParent.Index].instanceId = -1;
                    EntityUnload(entityParent.Index);
                    onUpdateDetails?.Invoke();
                    break;
            }

            foreach (EntityParent_Selection x in entityParents)
                if (SelectedCharacterParent != x)
                    x.SetState(EntityParent_Selection.State.NotSelected);
        }

        public override async Task EntityLoad(int index)
        {
            await base.EntityLoad(index);
            var entityParent = entityParents[index] as EntityParent_Selection;
            entityParent.SetValue(EntityParent_OnStateChange, false);
        }

        public void EntityUnload(int index) => entityParents[index].Unload();

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
