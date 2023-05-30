using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Object;
using static UnityEngine.Random;

namespace Extensions
{
    public static class WowExtensions
    {
        /// <summary>
        /// Destroy all children
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="immediate"></param>
        /// <returns></returns>
        public static void DestroyChildren(this Transform transform, bool immediate = false)
        {
            if (immediate)
            {
                for (var i = transform.childCount - 1; i >= 0; i--)
                {
                    var child = transform.GetChild(i);
                    if (DOTween.IsTweening(child)) child.DOKill();
                    DestroyImmediate(child.gameObject);
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (DOTween.IsTweening(child)) child.DOKill();
                    Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Get random element from list
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T">Any type</typeparam>
        /// <returns></returns>
        public static T GetRandomElement<T>(this List<T> list) =>
            list == null || list.Count == 0 ? default : list[Range(0, list.Count)];

        /// <summary>
        /// Activate the GameObject
        /// </summary>
        /// <param name="target"></param>
        public static void Activate(this Component target) => target.gameObject.SetActive(true);

        /// <summary>
        /// Deactivate the GameObject
        /// </summary>
        /// <param name="target"></param>
        public static void Deactivate(this Component target) => target.gameObject.SetActive(false);

        /// <summary>
        /// Activate the GameObject
        /// </summary>
        /// <param name="target"></param>
        public static void Activate(this GameObject target) => target.gameObject.SetActive(true);

        /// <summary>
        /// Deactivate the GameObject
        /// </summary>
        /// <param name="target"></param>
        public static void Deactivate(this GameObject target) => target.gameObject.SetActive(false);

        /// <summary>
        /// StopCoroutine
        /// </summary>
        /// <param name="coroutine"></param>
        /// <param name="owner">"Usually (this)"</param>
        public static void Stop(this Coroutine coroutine, MonoBehaviour owner)
        {
            if (coroutine != default) owner.StopCoroutine(coroutine);
        }
    }
}