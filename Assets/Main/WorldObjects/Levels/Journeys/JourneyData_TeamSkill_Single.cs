using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_TeamSkill", menuName = "GameData/JourneyData/TeamSkill")]
    public class JourneyData_TeamSkill_Single : JourneyData_TeamSkill
    {
        [SerializeField] SkillData skillData;

        public override Type JourneyType => skillData.rarity != RarityType.None ? Type.Positive : Type.Negative;
        public override string Title => GetTitle(skillData);
        public override string AdditionalDescription => GetDescription(skillData);

        public override void Execute() => AddSkill(skillData);
    }
}
