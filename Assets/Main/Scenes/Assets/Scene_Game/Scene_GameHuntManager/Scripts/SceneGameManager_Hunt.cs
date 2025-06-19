using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class SceneGameManager_Hunt : SceneGameManager
    {
        public static SceneGameManager_Hunt Instance_Hunt { get; private set; }

        public List<AssetReferenceMasterID<SkillData>> TeamSkillsAssetReference => SkillDataOverview_TeamSkill.Instance.dataList;
        public List<SkillData> TeamSkills => SkillDataOverview_TeamSkill.Instance.dataList.Select(x => x.Asset).ToList();

        protected override AssetReferenceMasterID<StageData> StageDataAssetReference => AppManager.Instance.userData.battleData.huntStageData;

        protected override void Awake()
        {
            Instance_Hunt = this;
            base.Awake(); 
        }

        protected override async Task Init()
        {
            var loadTask = TeamSkillsAssetReference.Select(x => x.Load());
            if (loadTask.Count() > 0)
                await Task.WhenAll(loadTask);
            await base.Init();
        }

        protected override void OnDestroy()
        {
            TeamSkillsAssetReference.ForEach(x => x.Unload());
            base.OnDestroy();
        }
    }
}
