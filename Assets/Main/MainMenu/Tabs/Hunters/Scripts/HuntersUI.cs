using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class HuntersUI : SingletonMonoBehaviour<HuntersUI>
    {
        [SerializeField] TeamManager_Selection teamManager_Selection;
        [SerializeField] TMP_InputField teamNameInput;
        [SerializeField] Button prevTeamButton;
        [SerializeField] Button nextTeamButton;
        [SerializeField] HuntersTab huntersTab;
        [SerializeField] GameObject tabs;

        public TeamCollection TeamCollection => UserData.Instance.teamCollection;
        [SerializeField, ReadOnly] SaveableTeamData_CharacterInstance teamData;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            teamNameInput.onEndEdit.AddListener(TextNameInput_OnUpdate);
            prevTeamButton.onClick.AddListener(PreviousTeam);
            nextTeamButton.onClick.AddListener(NextTeam);
            Init();
        }

        void Init()
        {
            huntersTab.Init(HunterPreviewItem_OnLoaded);
            RefreshTeam();
            tabs.SetActive(true);
        }

        private void HunterPreviewItem_OnLoaded(HunterPreviewItem item)
        {
            item.SetState(teamData.characterInstanceList.FirstOrDefault(x => x.instanceId == item.Asset.CharacterData.InstanceData.instanceId) != null ? HunterPreviewItem.State.Selected : HunterPreviewItem.State.NotSelected);
            item.onClick = HunterPreviewItem_OnClick;
        }

        private void HunterPreviewItem_OnClick(HunterPreviewItem item)
        {
            if (item.CurrentState.Value == HunterPreviewItem.State.Selected)
            {
                var slotIndex = teamData.characterInstanceList.FindIndex(x => x.instanceId == -1);
                if (slotIndex == -1)
                {
                    item.SetState(HunterPreviewItem.State.NotSelected);
                    return;
                }
                else
                {
                    teamData.characterInstanceList[slotIndex].instanceId = item.Asset.CharacterData.InstanceData.instanceId;
                    teamManager_Selection.CharacterLoad(slotIndex, item.Asset.CharacterData);
                }
            }
            else
            { 
                var slotIndex = teamData.characterInstanceList.FindIndex(x => x.instanceId == item.Asset.CharacterData.InstanceData.instanceId);
                teamData.characterInstanceList[slotIndex].instanceId = -1;
                teamManager_Selection.EntityUnload(teamManager_Selection.AliveEntityList[slotIndex]);
            }

            teamData.SetDirty();
        }

        public void RefreshTeam() => SetTeamIndex(TeamCollection.SelectedIndex);
        public void NextTeam() => SetTeamIndex(TeamCollection.SelectedIndex + 1);
        public void PreviousTeam() => SetTeamIndex(TeamCollection.SelectedIndex - 1);
        public void SetTeamIndex(int index)
        {
            TeamUnload();
            TeamCollection.SelectedIndex = index;
            teamData = SaveableData.Initialize(ref teamData, TeamCollection.SelectedTeamData, OnValueChange);
            UpdateVisuals();
        }

        public async void UpdateVisuals()
        {
            teamNameInput.text = teamData.teamName;
            await teamData.LoadAssets(SaveableCharacterData.AssetType.character);
            teamManager_Selection.Init(teamData);
            huntersTab.Refresh();
        }

        public void TeamUnload()
        {
            teamManager_Selection.Clear();
            if (teamData != null)
                teamData.UnloadAssets(SaveableCharacterData.AssetType.character);
        }

        private void TextNameInput_OnUpdate(string teamName)
        {
            teamData.teamName = teamName;
            teamData.SetDirty();
        }

        void OnValueChange()
        {
            
        }

        private void OnDestroy() => TeamUnload();
    }
}
