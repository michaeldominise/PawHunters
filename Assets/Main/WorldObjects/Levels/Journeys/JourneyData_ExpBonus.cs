using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_ExpBonus", menuName = "GameData/JourneyData/ExpBonus")]
    public class JourneyData_ExpBonus : JourneyData
    {
        [SerializeField] int exp = 30;

        public override Type JourneyType => Type.Positive;
        public override string Title => $"You gained {exp} exp!";
        public override string AdditionalDescription => "[title]";

        public override void Execute() => ExpUIManager.Instance.AddExp(exp, base.Execute);
    }
}
