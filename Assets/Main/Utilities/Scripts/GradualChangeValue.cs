using System;
using System.Collections;
using UnityEngine;

namespace PawHunters
{
    public static class GradualChangeValue
    {
        public class Status
        {
            public float startTime;
            public float startValue;
            public float endValue;
            public float duration;
            public float progress;
            public AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

            public float CurrentValue => Mathf.Lerp(startValue, endValue, progress);
            public bool IsDone => progress == 1;

            public Status(float startValue, float endValue, float duration, AnimationCurve animationCurve)
            {
                startTime = Time.time;
                this.startValue = startValue;
                this.endValue = endValue;
                this.duration = duration;
                this.animationCurve = animationCurve ?? this.animationCurve;
            }
        }

        public static Coroutine Execute(float startValue, float endValue, float duration, Action<Status> onUpdate, AnimationCurve animationCurve = null)
        {
            if (!GameManager.Instance)
            {
                Debug.LogWarning($"{nameof(GameManager)} does not exist in the scene!");
                return null;
            }

            return GameManager.Instance.StartCoroutine(_Execute(new(startValue, endValue, duration, animationCurve), onUpdate));
        }

        static IEnumerator _Execute(Status status, Action<Status> onUpdate)
        {
            while (status.startTime + status.duration > Time.time)
            {
                status.progress = status.animationCurve.Evaluate((Time.time - status.startTime) / status.duration);
                onUpdate?.Invoke(status);
                yield return null;
            }

            status.progress = 1;
            onUpdate?.Invoke(status);
        }
    }
}
