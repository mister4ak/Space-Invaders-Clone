using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public sealed class GameUI : MonoBehaviour
    {
        public event Action OnRestartButtonClicked;
        public event Action OnStartWindowShowed;
        
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private LoseWindow _loseWindow;
        [SerializeField] private GameWindow _gameWindow;

        public void Initialize()
        {
            _gameWindow.Initialize();
            _startWindow.Initialize();

            _loseWindow.OnRestartClicked += RestartClicked;
        }

        public void ShowStartWindow()
        {
            _startWindow.Show();
            _startWindow.ShowFinished += StartWindowShowFinished;
        }

        public void ShowLoseWindow()
        {
            _gameWindow.Hide();
            _loseWindow.Show();
        }

        public void AddScore(int rewardScore)
        {
            _gameWindow.AddScore(rewardScore);
        }

        public void SetHealth(int playerHealth)
        {
            _gameWindow.SetHealth(playerHealth);
        }

        private void StartWindowShowFinished()
        {
            _startWindow.ShowFinished -= StartWindowShowFinished;
            
            _startWindow.Hide();
            _gameWindow.Show(() => OnStartWindowShowed?.Invoke());
        }

        private void RestartClicked()
        {
            _loseWindow.Hide();
            _gameWindow.Show(() => OnRestartButtonClicked?.Invoke());
        }
    }
}