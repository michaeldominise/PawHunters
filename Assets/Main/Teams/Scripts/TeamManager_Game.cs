using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace PawHunters
{
    public class TeamManager_Game : TeamManager
    {
        public Vector3 offset;

        [Button]
        public void Kill() => AliveEntityList.ForEach(x => x.EntityHealthController.Kill());
        public override void Init(SaveableTeamData teamData)
        {
            base.Init(teamData);
            ExecuteSetupSkills();
        }

        public void ExecuteSetupSkills()
        {
            foreach (var entity in AliveEntityList)
                _ = entity.EntitySkillsController.Execute(GameActionTriggersManager.TriggerType.SetupPhase);
        }
    }
}
