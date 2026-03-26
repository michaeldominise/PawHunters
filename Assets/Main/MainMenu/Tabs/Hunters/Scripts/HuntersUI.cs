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
        [SerializeField] GameObject container;
        [SerializeField] TeamManager_Selection teamManager_Selection;
        [SerializeField] HuntersTab huntersTab;
        [SerializeField] EquipmentsTab equipmentsTab;
        [SerializeField] GameObject tabs;

        public TeamCollection TeamCollection => UserData.Instance.teamCollection;
        [ReadOnly] SaveableTeamData_CharacterInstance TeamDataInstance => teamManager_Selection.TeamDataInstance;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            Init();
        }

        void Init()
        {
            huntersTab.Init(HunterPreviewItem_OnLoaded);
            equipmentsTab.Init(EquipmentPreviewItem_OnLoaded);
            teamManager_Selection.Init(TeamCollection, huntersTab.Refresh);
            tabs.SetActive(true);
        }

        private void EquipmentPreviewItem_OnLoaded(EntityPreviewItem<SaveableEquipmentData> item)
        {
            item.SetState(TeamDataInstance.equipmentInstanceList.FirstOrDefault(x => x.instanceId == item.Asset.Data.InstanceData.instanceId) != null ? EquipmentPreviewItem.State.Selected : EquipmentPreviewItem.State.NotSelected);
            item.onClick = EquipmentPreviewItem_OnClick;
        }

        private void HunterPreviewItem_OnLoaded(EntityPreviewItem<SaveableCharacterData> item)
        {
            item.SetState(TeamDataInstance.characterInstanceList.FirstOrDefault(x => x.instanceId == item.Asset.Data.InstanceData.instanceId) != null ? HunterPreviewItem.State.Selected : HunterPreviewItem.State.NotSelected);
            item.onClick = HunterPreviewItem_OnClick;
        }

        private void EquipmentPreviewItem_OnClick(EntityPreviewItem<SaveableEquipmentData> item)
        {
            item.SetState(EquipmentPreviewItem.State.NotSelected);
            ShowStatsPreview(item.Data);
        }

        private void HunterPreviewItem_OnClick(EntityPreviewItem<SaveableCharacterData> item)
        {
            var slotIndex = teamManager_Selection.SelectedCharacterParent == null ? -1 : teamManager_Selection.SelectedCharacterParent.Index;
            if (slotIndex == -1)
            {
                ShowStatsPreview(item.Data);
                return;
            }

            huntersTab.ScrollRectPoolHandler.activeList.FirstOrDefault(x => x != item && x.Data.InstanceData.instanceId == TeamDataInstance.characterInstanceList[slotIndex].instanceId)?.SetState(EntityPreviewItem<SaveableCharacterData>.State.NotSelected);
            item.SetState(HunterPreviewItem.State.Selected);
            var unloadIndex = TeamDataInstance.characterInstanceList.FindIndex(x => x.instanceId == item.Asset.Data.InstanceData.instanceId);
            if(unloadIndex != -1)
            {
                TeamDataInstance.characterInstanceList[unloadIndex].instanceId = -1;
                teamManager_Selection.EntityUnload(unloadIndex);
            }

            TeamDataInstance.characterInstanceList[slotIndex].instanceId = item.Asset.Data.InstanceData.instanceId;
            _ = teamManager_Selection.EntityLoad(slotIndex);
            teamManager_Selection.SelectedCharacterParent = null;

            TeamDataInstance.SetDirty();
        }

        void ShowStatsPreview(SaveableDataEntity saveableDataEntity)
        {
            EntityStatsPreview.Instance.Show(saveableDataEntity, null);
            container.SetActive(false);
            BackNavigationHandler.Add(this, () =>
            {
                EntityStatsPreview.Instance.Show(false);
                container.SetActive(true);
            }, BackNavigationHandler.BackHandlerMode.Execute);
        }
    }
}
