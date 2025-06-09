using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabHaven.PawHunters
{
    public class SkillData : MonoBehaviour
    {
        public string title;
        public string familyName;
        [TextArea] public string description;
        public Sprite icon;
        public GameActionTriggersManager.TriggerType trigger = GameActionTriggersManager.TriggerType.Instant;
        public List<SkillTargetData> skillTargets;
        public List<SkillCustomCondition> customConditions;

        public async Task Execute(GameActionTriggersManager.TriggerType trigger, EntityMainController caster, object triggerSource = null)
        {
            if (!caster.IsAlive
                || !this.trigger.HasFlag(trigger)
                || customConditions.FirstOrDefault(x => !x.IsVaild(caster, triggerSource)))
                return;

            Debug.Log($"{caster.name}:{(bool)caster.TeamManager_GamePlayer} execute Skill:'{title}'");

            foreach (var skillTarget in skillTargets)
                await skillTarget.Execute(caster);
        }

        [Button]
        public async Task Execute()
        {
            Debug.Log($"[SkillData.Execute] System execute Skill:'{title}'");

            foreach (var skillTarget in skillTargets)
                await skillTarget.Execute(null);
        }
    }
}
