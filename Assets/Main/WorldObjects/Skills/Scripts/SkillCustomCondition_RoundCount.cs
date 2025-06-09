using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class SkillCustomCondition_RoundCount : SkillCustomCondition
    {
        public enum ExecuteType
        {
            NotRepeating,
            Repeating,
            RepeatingAndExcuteAtFirstRound,
        }

        [SerializeField] int roundCount = 2;
        [SerializeField] ExecuteType type;

        public override bool IsVaild(EntityMainController caster, object triggerSource) => IsValid();
        public override bool IsVaild(EntityMainController caster, StatusEffectDataHandler triggerSource) => IsValid();
        public override bool IsVaild(EntityMainController caster, EntityMainController target) => IsValid();

        bool IsValid()
        {
            if (type == ExecuteType.NotRepeating)
                return BattleManager.Instance.CurrentRound == roundCount;
            else if (type == ExecuteType.RepeatingAndExcuteAtFirstRound)
                return BattleManager.Instance.CurrentRound % roundCount == 0;
            else
                return BattleManager.Instance.CurrentRound != 0 && (BattleManager.Instance.CurrentRound - roundCount) % roundCount == 0;
        }
    }
}
