using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public static class OnFinishCounter
    {
        public static void Execute(Action onFinish, int reqCount, ref int finishCount)
        {
            finishCount++;
            if (finishCount == reqCount)
                onFinish?.Invoke();
        }
    }
}
