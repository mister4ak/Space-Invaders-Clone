using System;
using UnityEngine;

namespace UI
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [Header("Base Window")]
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected float _fadeDuration = 0.25f;

        public abstract void Show(Action finished = null);

        public abstract void Hide(Action finished = null);
    }
}