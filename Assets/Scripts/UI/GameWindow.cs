using System;
using System.Collections.Generic;
using Extensions;
using ScriptableObjects.Classes;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameWindow : BaseWindow
    {
        [Space]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private HealthImage _healthImagePrefab;
        [SerializeField] private RectTransform _healthImagesContainer;
        [SerializeField] private PlayerData _playerData;

        private readonly List<HealthImage> _healthImages = new();
        private int _scoreCounter;

        public void Initialize()
        {
            GenerateHealthImages();
        }

        public override void Show(Action finished = null)
        {
            _canvas.Activate();
            
            _canvasGroup.alpha = 0;
            _canvasGroup.Show(_fadeDuration, 
                callback: () => finished?.Invoke());
            
            ResetScore();
        }

        public override void Hide(Action finished = null)
        {
            _canvasGroup.Hide(_fadeDuration, 
                callback: () => _canvas.Deactivate());
        }

        public void SetHealth(int health)
        {
            _healthText.text = health.ToString();

            for (int i = 0; i < _healthImages.Count; i++)
            {
                if (i < health - 1)
                    _healthImages[i].Activate();
                else
                    _healthImages[i].Deactivate();
            }
        }

        public void AddScore(int rewardScore)
        {
            _scoreCounter += rewardScore;
            UpdateScoreText();
        }

        public void ResetScore()
        {
            _scoreCounter = 0;
            UpdateScoreText();
        }

        private void GenerateHealthImages()
        {
            for (int i = 0; i < _playerData.MaxHealth - 1; i++)
            {
                var healthImage = Instantiate(_healthImagePrefab, _healthImagesContainer);
                _healthImages.Add(healthImage);
            }
            
            _healthText.text = _playerData.MaxHealth.ToString();
        }

        private void UpdateScoreText()
        {
            _scoreText.text = _scoreCounter.ToString();
        }
    }
}