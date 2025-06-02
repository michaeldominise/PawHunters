using System;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    [Serializable]
    public class StatusEffectDataHandler
    {
        public StatusEffectData data;
        public EntityMainController caster;
        public EntityMainController target;
        public int expirationCountdown;
        public float cachedValue;

        public StatusEffectDataHandler(StatusEffectData data, EntityMainController caster, EntityMainController target)
        {
            this.data = data;
            this.caster = caster;
            this.target = target;
            data.transform.SetParent(target.EntityStatusEffectController.transform);
            cachedValue = data.GetValue(caster, target);
            expirationCountdown = data.ExpirationCount;
        }

        public async Task Execute()
        {
            if (data.TargetHealthStatus == SkillTargetData.HealthStatusType.Alive && !target.IsAlive)
                return;
            if (data.TargetHealthStatus == SkillTargetData.HealthStatusType.Dead && target.IsAlive)
                return;
            if (data.CustomConditions.FirstOrDefault(x => !x.IsVaild(caster, target)))
                return;

            if (caster)
                await data.WaitStopMoving(this);
            await data.PlayExecuteVisual(this);
            var executeValue = await data.Execute(this);
            var executeLog = caster ? $"{caster.name}:{(bool)caster.TeamManager_GamePlayer}" : "System";
            Debug.Log($"[StatusEffectDataHandler.Execute] {executeLog} execute StatusEffect:'{data.Title}:{executeValue}' to {target.name}:{(bool)target.TeamManager_GamePlayer}");

            await GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.StatusEffectExecuted, caster);
            await target.EntitySkillsController.Execute(GameActionTriggersManager.TriggerType.StatusEffectExecutedToTarget, this);
        }

        public async Task<bool> Expire()
        {
            expirationCountdown--;
            if (expirationCountdown > 0)
                await data.PlayExpireCountdownVisual(this);
            else
            {
                await data.PlayExpireDoneVisual(this);
                await data.Expire(this);
            }
            return expirationCountdown <= 0;
        }
    }
}
