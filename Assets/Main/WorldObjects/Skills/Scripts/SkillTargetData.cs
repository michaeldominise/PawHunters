using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHaven.PawHunters
{
    public class SkillTargetData : MonoBehaviour
    {
        public enum GroupType
        {
            All = (1 << 2) - 1,
            Allies = 1 << 0,
            Opponent = 1 << 1,
            Caster = 1 << 2,
            TriggerSource = 1 << 3,
        }

        public enum HealthStatusType
        {
            Alive = 1 << 0,
            Dead = 1 << 1,
            All = (1 << 2) - 1,
        }

        public enum GroupOrderType { Accending, Decending, Middle, Random }

        [SerializeField] List<StatusEffectData> statusEffects;
        [SerializeField] GroupType targetGroupType = GroupType.Allies;
        [SerializeField] GroupOrderType groupOrderType;
        [SerializeField] HealthStatusType healthStatusType = HealthStatusType.Alive;
        [SerializeField] int targetCount = 1;
        [SerializeField] SkillVisualEffect[] executeSkillVFX;
        [SerializeField] List<SkillCustomCondition> customConditions;

        public async Task Execute(EntityMainController caster, StatusEffectDataHandler triggerSource = null)
        {
            var targets = GetTargetEntities(caster, triggerSource);
            await SkillVisualEffect.PlayVisual(SkillVisualEffect.State.Start, executeSkillVFX, caster, targets.ToArray());

            foreach (var target in targets)
                foreach (var statusEffect in statusEffects)
                    await target.EntityStatusEffectController.ApplyStatusEffect(statusEffect, caster);

            await SkillVisualEffect.PlayVisual(SkillVisualEffect.State.End, executeSkillVFX, caster, targets.ToArray());
        }

        public List<EntityMainController> GetTargetEntities(EntityMainController caster, StatusEffectDataHandler triggerSource = null)
        {
            var targetGroup = new List<EntityMainController>();

            if (targetGroupType.HasFlag(GroupType.Allies))
                targetGroup.AddRange(caster != null && caster.TeamManager_GamePlayer ? TeamManager_GamePlayer.Instance.EntityList : TeamManager_GameEnemy.Instance.EntityList);
            if (targetGroupType.HasFlag(GroupType.Opponent))
                targetGroup.AddRange(caster != null && caster.TeamManager_GamePlayer ? TeamManager_GameEnemy.Instance.EntityList : TeamManager_GamePlayer.Instance.EntityList);
            if (targetGroupType.HasFlag(GroupType.TriggerSource) && triggerSource != null)
                targetGroup.Add(triggerSource.caster);
            if (targetGroupType.HasFlag(GroupType.Caster))
                targetGroup.Add(caster);

            targetGroup = targetGroup.Where(x =>
                {
                    if (healthStatusType == HealthStatusType.Alive && !x.IsAlive)
                        return false;
                    else if (healthStatusType == HealthStatusType.Dead && x.IsAlive)
                        return false;
                    if (customConditions.FirstOrDefault(condition => !condition.IsVaild(caster, x)))
                        return false;
                    return true;
                }).ToList();

            if (targetGroup.Count == 0)
                return targetGroup;

            var index = Mathf.Ceil(targetGroup.Count / 2f) - 1;
            targetGroup = groupOrderType switch
            {
                GroupOrderType.Decending => targetGroup.OrderByDescending(x =>
                {
                    index++;
                    return index;
                }).ToList(),
                GroupOrderType.Middle => targetGroup.OrderBy(x =>
                {
                    index = Mathf.Repeat(index + 1, targetGroup.Count);
                    return index;
                }).ToList(),
                GroupOrderType.Random => GetRandom(targetGroup),
                _ => targetGroup,
            };

            return targetGroup.GetRange(0, Mathf.Min(targetCount, targetGroup.Count));
        }

        List<T> GetRandom<T>(List<T> list)
        {
            var newList = new List<T>(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                var rnd = Random.Range(0, list.Count);
                newList[x] = list[rnd];
                list.RemoveAt(rnd);
            }
            return newList;
        }
    }
}
