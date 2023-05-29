using System;
using Common;
using Enemies;
using ScriptableObjects.Classes;
using UI;
using UnityEngine;

public sealed class Game : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyHandler _enemyHandler;
    [SerializeField] private Player _player;
    [SerializeField] private GameUI _gameUI;
    [Header("Delays")]
    [SerializeField] private float _delayAfterPlayerDamage = 1.5f;
    [SerializeField] private float _delayAfterLose = 2f;
    [SerializeField] private float _delayAfterWin = 1f;
        
    private void Awake()
    {
        _enemyHandler.Initialize();
        _gameUI.Initialize();
        _player.Initialize();

        _gameUI.OnRestartButtonClicked += RestartLevel;
        _gameUI.OnStartWindowShowed += StartLevel;
            
        _enemyHandler.OnEnemyReachedBottomBorder += LoseGame;
        _enemyHandler.OnAllEnemiesDied += WinGame;
        _enemyHandler.OnEnemyDied += EnemyDied;

        _player.OnTakeDamage += PlayerTakeDamage;
        _player.OnDied += LoseGame;
        
        _gameUI.ShowStartWindow();
    }

    private void PlayerTakeDamage()
    {
        _enemyHandler.StopMovement();
        
        StartCoroutine(Helper.WaitCoroutine(_delayAfterPlayerDamage, () =>
        {
            _gameUI.SetHealth(_player.Health);
            _enemyHandler.StartMovement();
            _player.Respawn();
        }));
    }

    private void LoseGame()
    {
        _enemyHandler.StopMovement();

        StartCoroutine(Helper.WaitCoroutine(_delayAfterLose, () =>
        {
            _gameUI.ShowLoseWindow();
        }));
    }

    private void EnemyDied(EnemyData enemyData)
    {
        _gameUI.AddScore(enemyData.RewardScore);
    }

    private void RestartLevel()
    {
        _player.IncreaseHealth();
        _gameUI.SetHealth(_player.Health);
        StartLevel();
    }

    private void StartLevel()
    {
        _enemyHandler.SpawnEnemies();
        _player.StartMovement();
    }

    private void WinGame()
    {
        _player.StopMovement();
            
        StartCoroutine(Helper.WaitCoroutine(_delayAfterWin, () =>
        {
            _enemyHandler.Reset();
            StartLevel();
        }));
    }

    private void OnDestroy()
    {
        _gameUI.OnRestartButtonClicked -= RestartLevel;
        _gameUI.OnStartWindowShowed -= StartLevel;
            
        _enemyHandler.OnEnemyReachedBottomBorder -= LoseGame;
        _enemyHandler.OnAllEnemiesDied -= WinGame;
        _enemyHandler.OnEnemyDied -= EnemyDied;

        _player.OnTakeDamage -= PlayerTakeDamage;
        _player.OnDied -= LoseGame;
    }
}