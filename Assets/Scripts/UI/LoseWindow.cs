using System;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoseWindow : BaseWindow
    {
        public event Action OnRestartClicked;
        
        [Space(20)]
        [SerializeField] private Button _restartButton;
        
        public override void Show(Action finished = null)
        {
            _canvas.Activate();
            
            _canvasGroup.alpha = 0;
            _canvasGroup.Show(_fadeDuration, callback: () =>
            {
                _restartButton.interactable = true;
                _restartButton.onClick.AddListener(OnRestartButtonClicked);
            });
        }

        public override void Hide(Action finished = null)
        {
            _restartButton.interactable = false;
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _canvasGroup.Hide(_fadeDuration, callback: () => _canvas.Deactivate());
        }

        private void OnRestartButtonClicked()
        {
            OnRestartClicked?.Invoke();
        }
    }
}