using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class SkillVisualEffect_Movement : SkillVisualEffect
    {
        [SerializeField] bool isRunning = true;

        public override async Task PlayStart(EntityMainController caster, params EntityMainController[] targets)
            => await caster.EntityMovementController.MoveToFront(targets.FirstOrDefault(), isRunning);

        public override async Task PlayEnd(EntityMainController caster, params EntityMainController[] targets)
            => await caster.EntityMovementController.MoveToStartPosition(isRunning);
    }
}
