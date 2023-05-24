using DG.Tweening;
using UnityEngine;

namespace Extensions
{
	public static class TransformEx
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
					Object.DestroyImmediate(child.gameObject);
				}
			}
			else
			{
				foreach (Transform child in transform)
				{
					if (DOTween.IsTweening(child)) child.DOKill();
					Object.Destroy(child.gameObject);
				}
			}
		}
	}
}