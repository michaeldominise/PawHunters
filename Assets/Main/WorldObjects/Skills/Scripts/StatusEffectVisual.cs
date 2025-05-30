using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class StatusEffectVisual : MonoBehaviour
    {
        [SerializeField] Animation anim;

        [SerializeField] Vector3 casterOffset;
        [SerializeField] Vector3 targetOffset;

        [SerializeField] string executeAnimation = "Execute";
        [SerializeField] float executeDuration;
        [SerializeField] string expireAnimation = "Expire";
        [SerializeField] float expireDuration;

        [Button] public async Task Execute() => await PlayVisual(executeAnimation, executeDuration);
        [Button] public async Task Expire() => await PlayVisual(expireAnimation, expireDuration);
        async Task PlayVisual(string clipName, float duration)
        {
            if (!anim || anim.GetClip(clipName))
                return;

            anim.Stop();
            anim.Play(clipName);
            await Task.Delay((int)(duration * 1000));
        }
    }
}
