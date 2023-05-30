using System;
using DG.Tweening;
using UnityEngine;

namespace Extensions
{
	public static class CanvasGroupExtension
	{
		public static void Set(this CanvasGroup canvasGroup, float alpha, bool interactable, bool blocksRaycasts)
		{
			canvasGroup.DOKill();
			canvasGroup.alpha = alpha;
			canvasGroup.interactable = interactable;
			canvasGroup.blocksRaycasts = blocksRaycasts;
		}

		/// <param name="duration">Duration of fading in, cannot be negative</param>
		/// <param name="delay">Delay before canvas group show, cannot be negative</param>
		public static void Show(this CanvasGroup canvasGroup, float duration = 0, float delay = 0, bool useSetActive = false,
			Action callback = null)
		{
			if (duration < 0) throw new ArgumentException("Value cannot be negative", nameof(duration));

			if (delay < 0) throw new ArgumentException("Value cannot be negative", nameof(delay));

			if (useSetActive)
			{
				canvasGroup.gameObject.SetActive(true);
			}

			canvasGroup.DOKill();
			if (duration == 0 && delay == 0) canvasGroup.alpha = 1;
			canvasGroup.DOFade(1, duration)
				.SetDelay(delay)
				.SetLink(canvasGroup.gameObject)
				.OnComplete(() =>
				{
					canvasGroup.interactable = true;
					canvasGroup.blocksRaycasts = true;
					callback?.Invoke();
				});
		}

		/// <param name="duration">Duration of fading out, cannot be negative</param>
		/// <param name="delay">Delay before canvas group hide, cannot be negative</param>
		public static void Hide(this CanvasGroup canvasGroup, float duration = 0, float delay = 0, bool useSetActive = false,
			Action callback = null)
		{
			if (duration < 0) throw new ArgumentException("Value cannot be negative", nameof(duration));

			if (delay < 0) throw new ArgumentException("Value cannot be negative", nameof(delay));

			canvasGroup.DOKill();
			if (duration == 0 && delay == 0) canvasGroup.alpha = 0;
			canvasGroup.DOFade(0, duration)
				.SetDelay(delay)
				.SetLink(canvasGroup.gameObject)
				.OnStart(() =>
				{
					canvasGroup.interactable = false;
					canvasGroup.blocksRaycasts = false;
				})
				.OnComplete(() =>
				{
					if (useSetActive)
					{
						canvasGroup.gameObject.SetActive(false);
					}
					callback?.Invoke();
				});
		}
	}
}