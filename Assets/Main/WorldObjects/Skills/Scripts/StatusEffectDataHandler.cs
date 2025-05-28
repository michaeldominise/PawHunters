using System;
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
        public EntityMainController triggerSource;
        public int expirationCountdown;
        public float cachedValue;

        public EntityMainController ExecuteToEntity =>
             data.ExecuteTo switch
             {
                 StatusEffectData.ExecuteTargetType.Caster => caster,
                 StatusEffectData.ExecuteTargetType.TriggerSource => triggerSource,
                 _ => target
             };

        public StatusEffectDataHandler(StatusEffectData data, EntityMainController caster, EntityMainController target)
        {
            this.data = data;
            this.caster = caster;
            this.target = target;
            data.transform.SetParent(target.EntityStatusEffectController.transform);
            cachedValue = data.GetValue(caster, target, null);
            expirationCountdown = data.ExpirationCount;
        }

        void RefreshValue() => cachedValue = data.GetValue(caster, target, triggerSource);

        public async Task Execute()
        {
            if (data.RefreshValueOnExecute)
                RefreshValue();

            await data.PlayExecuteVisual(this);
            await data.Execute(this);
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
