using System;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_TeamAddSkill", menuName = "GameData/JourneyData/TeamAddSkill")]
    public class JourneyData_TeamSkill : JourneyData
    {
        public enum BehaviourType { Execute, AddToSkillList }

        [SerializeField] SkillData skillData;
        [SerializeField] BehaviourType behaviourType;
        [SerializeField] bool isPositive = true;

        public override Type JourneyType => isPositive ? Type.Positive : Type.Negative;
        public override string Title => skillData.description;
        public override string AdditionalDescription => "[title]";

        public override void Execute()
        {
            if(behaviourType == BehaviourType.Execute)
                _ = skillData.Execute(GameActionTriggersManager.TriggerType.Instant, TeamManager_GamePlayer.Instance.EntityMainController_Team);
            else
                TeamManager_GamePlayer.Instance.EntityMainController_Team.EntitySkillsController.AddSkill(skillData);
            OnTransitionFinished();
        }
    }
}
