using System;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_TeamSkill", menuName = "GameData/JourneyData/TeamSkill")]
    public abstract class JourneyData_TeamSkill : JourneyData
    {
        protected void AddSkill(SkillData skillData)
        {
            TeamManager_GamePlayer.Instance.AddSkill(skillData);
            JourneyButtons.Instance.Init(response => base.End(), ButtonLabel);
        }

        public static string GetTitle(SkillData skillData)
        {
            if (skillData.rarity == RarityType.None)
                return skillData.description;
            else if(skillData.trigger.HasFlag(GameActionTriggersManager.TriggerType.SetupPhase))
                return skillData.description;
            else
                return skillData.title;
        }

        public static string GetDescription(SkillData skillData)
        {
            if (skillData.rarity == RarityType.None)
                return $"You are unfortunate. [title]";
            else if (skillData.trigger.HasFlag(GameActionTriggersManager.TriggerType.SetupPhase))
                return $"[title]";
            else
                return $"You learned {skillData.rarity} skill [title].";
        }
    }
}
