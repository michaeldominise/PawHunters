using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace PawHunters
{
    public class SkillData : MonoBehaviour
    {
        [TextArea]
        public string description;
        public Sprite icon;
        public List<SkillTargetData> skillTargets;

        public async Task Execute(EntityMainController caster)
        {
            foreach (var skillTarget in skillTargets)
                await skillTarget.Execute(caster);
        }
    }
}
