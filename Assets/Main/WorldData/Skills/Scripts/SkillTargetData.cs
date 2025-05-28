using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace PawHunters
{
    public class SkillTargetData : MonoBehaviour
    {
        [System.Flags]
        public enum GroupType
        {
            None,
            Allies = 1 << 0,
            Opponent = 1 << 1,
            All = (1 << 2) - 1,
        }
        public enum GroupOrderType { Accending, Decending, Middle, Random }

        [SerializeField] List<StatusEffectData> statusEffects;
        [SerializeField] GroupType targetGroupType;
        [SerializeField] GroupOrderType groupOrderType;
        [SerializeField] int targetCount = 1;

        public async Task Execute(EntityMainController caster)
        {
            var targets = GetTargetEntities(caster);
            foreach (var target in targets)
                foreach(var statusEffect in statusEffects)
                    await target.EntityStatusEffectController.ApplyStatusEffect(statusEffect, caster);
        }

        List<EntityMainController> GetTargetEntities(EntityMainController caster)
        {
            var targetGroup = new List<EntityMainController>();

            if (targetGroupType.HasFlag(GroupType.Allies))
                targetGroup.AddRange(caster.TeamManager_GamePlayer ? TeamManager_GamePlayer.Instance.AliveEntityList : TeamManager_GameEnemy.Instance.AliveEntityList);
            if (targetGroupType.HasFlag(GroupType.Opponent))
                targetGroup.AddRange(caster.TeamManager_GamePlayer ? TeamManager_GameEnemy.Instance.AliveEntityList : TeamManager_GamePlayer.Instance.AliveEntityList);

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

            return targetGroup.GetRange(0, Mathf.Max(targetCount, targetGroup.Count));
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
