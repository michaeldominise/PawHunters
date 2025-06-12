using System;
using System.Collections;
using UnityEngine;

namespace LabHaven.PawHunters
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
            public Coroutine coroutine;

            public float CurrentValue => Mathf.LerpUnclamped(startValue, endValue, progress);
            public float CurrentValueClamped => Mathf.Lerp(startValue, endValue, progress);
            public bool IsDone => progress == 1;

            public Status(float startValue, float endValue, float duration, AnimationCurve animationCurve)
            {
                startTime = Time.time;
                this.startValue = startValue;
                this.endValue = endValue;
                this.duration = duration;
                this.animationCurve = animationCurve ?? this.animationCurve;
            }

            public void Stop() => GameManager.Instance.StopCoroutine(coroutine);
        }

        public static Status Execute(float startValue, float endValue, float duration, Action<Status> onUpdate, AnimationCurve animationCurve = null)
        {
            if (!GameManager.Instance)
            {
                Debug.LogWarning($"{nameof(GameManager)} does not exist in the scene!");
                return null;
            }

            var status = new Status(startValue, endValue, duration, animationCurve);
            status.coroutine = GameManager.Instance.StartCoroutine(_Execute(status, onUpdate));
            return status;
        }

        static IEnumerator _Execute(Status status, Action<Status> onUpdate)
        {
            var time = 0f;
            while (status.duration > time)
            {
                status.progress = status.animationCurve.Evaluate(time / status.duration);
                onUpdate?.Invoke(status);
                yield return null;
                time += Time.deltaTime;
            }

            status.progress = 1;
            onUpdate?.Invoke(status);
        }
    }
}
