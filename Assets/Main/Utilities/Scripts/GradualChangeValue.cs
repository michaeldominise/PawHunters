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
            public AnimationCurve animationCurve;

            float currentValue;
            public float CurrentValue
            {
                get => currentValue;
                set
                {
                    currentValue = value;
                    progress = value / endValue;
                }
            }

            float progress;
            public float Progress
            {
                get => progress;
                set
                {
                    progress = value;
                    currentValue = Mathf.Lerp(startValue, endValue, value);
                }
            }

            public bool IsDone => startValue == endValue;

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
                status.Progress = status.animationCurve.Evaluate((Time.time - status.startTime) / status.duration);
                onUpdate?.Invoke(status);
                yield return null;
            }

            status.Progress = 1;
            onUpdate?.Invoke(status);
        }
    }
}
