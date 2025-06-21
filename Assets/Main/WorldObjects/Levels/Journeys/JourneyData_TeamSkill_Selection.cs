using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_TeamSkill_Selection", menuName = "GameData/JourneyData/TeamSkill_Selection")]
    public class JourneyData_TeamSkill_Selection : JourneyData_TeamSkill
    {
        [SerializeField] SkillData[] skillDataList;

        public override Type JourneyType => Type.Positive;
        public override string Title => "You select a bonus skill.";
        public override string AdditionalDescription => "[title]";

        public override void End() => SkillSelectionUIManager.Instance.Init(SkillDataSelected, skillDataList);
        void SkillDataSelected(SkillData skillData)
        {
            JourneyLogsUIManager.Instance.Spawn(skillData.rarity == RarityType.None ? Type.Negative : Type.Default, GetTitle(skillData), GetDescription(skillData));
            AddSkill(skillData);
        }
    }
}
