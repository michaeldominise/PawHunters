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
        [SerializeField] HuntersTab huntersTab;
        [SerializeField] GameObject tabs;

        public TeamCollection TeamCollection => UserData.Instance.teamCollection;
        [SerializeField, ReadOnly] SaveableTeamData_CharacterInstance TeamDataInstance => teamManager_Selection.TeamDataInstance;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            Init();
        }

        void Init()
        {
            huntersTab.Init(HunterPreviewItem_OnLoaded);
            teamManager_Selection.Init(TeamCollection, huntersTab.Refresh);
            tabs.SetActive(true);
        }

        private void HunterPreviewItem_OnLoaded(HunterPreviewItem item)
        {
            item.SetState(TeamDataInstance.characterInstanceList.FirstOrDefault(x => x.instanceId == item.Asset.CharacterData.InstanceData.instanceId) != null ? HunterPreviewItem.State.Selected : HunterPreviewItem.State.NotSelected);
            item.onClick = HunterPreviewItem_OnClick;
        }

        private void HunterPreviewItem_OnClick(HunterPreviewItem item)
        {
            if (item.CurrentState.Value == HunterPreviewItem.State.Selected)
            {
                var slotIndex = TeamDataInstance.characterInstanceList.FindIndex(x => x.instanceId == -1);
                if (slotIndex == -1)
                {
                    item.SetState(HunterPreviewItem.State.NotSelected);
                    return;
                }
                else
                {
                    TeamDataInstance.characterInstanceList[slotIndex].instanceId = item.Asset.CharacterData.InstanceData.instanceId;
                    teamManager_Selection.CharacterLoad(slotIndex, item.Asset.CharacterData);
                }
            }
            else
            { 
                var slotIndex = TeamDataInstance.characterInstanceList.FindIndex(x => x.instanceId == item.Asset.CharacterData.InstanceData.instanceId);
                TeamDataInstance.characterInstanceList[slotIndex].instanceId = -1;
                teamManager_Selection.EntityUnload(teamManager_Selection.AliveEntityList[slotIndex]);
            }

            TeamDataInstance.SetDirty();
        }
    }
}
