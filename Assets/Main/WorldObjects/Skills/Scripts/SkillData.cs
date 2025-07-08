using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class SkillData : MonoBehaviour
    {
        [System.Serializable]
        public class Descriptive
        {
            public enum Key { ElementEffectivity, ElementResistance }

            public Key key;
            public ElementType elementType;
            public float value;

            public Descriptive() { }
            public Descriptive(Key key, float value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public string title;
        public string familyName;
        [TextArea] public string description;
        public Sprite icon;
        public RarityType rarity = RarityType.Common;
        public GameActionTriggersManager.TriggerType trigger = GameActionTriggersManager.TriggerType.Instant;
        public List<SkillTargetData> skillTargets;
        public List<SkillCustomCondition> customConditions;
        public List<Descriptive> descriptives;

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
