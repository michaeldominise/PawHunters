using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class SceneMainMenuManager : SingletonMonoBehaviour<SceneMainMenuManager>
    {
        public SaveableTeamData PlayerTeamData => AppManager.Instance?.userData.playerTeamData;
        public StageData StageData => StageDataAssetReference.Asset;
        protected AssetReferenceMasterID<StageData> StageDataAssetReference => AppManager.Instance.userData.battleData.huntStageData;

        IEnumerator Start()
        {
            yield return null;
            yield return null;
            _ = Init();
        }

        protected async virtual Task Init()
        {
            await StageDataAssetReference.Load();
            await StageData.data.LoadAssets(StageData.AssetType.environmentItem2);
            await PlayerTeamData.LoadAssets(SaveableCharacterData.AssetType.character);
            EnvironmentManager.Instance.Init(StageData.data.environmentItem2.Asset);
            TeamManager_GamePlayer.Instance.Init(PlayerTeamData);
            InitialUI.Instance.Init(StageData.title);
        }

        protected virtual void OnDestroy()
        {
            if (StageData == null)
                return;
            StageData.data.UnloadAssets(StageData.AssetType.environmentItem2);
            PlayerTeamData.UnloadAssets(SaveableCharacterData.AssetType.character);
            AppManager.Instance.userData.battleData.huntStageData.Unload();
        }
    }
}
