using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class Helper
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();

        public static IEnumerator WaitCoroutine(float duration, Action finished = null)
        {
            yield return GetWait(duration);
            finished?.Invoke();
        }

        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait))
                return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
    }
}