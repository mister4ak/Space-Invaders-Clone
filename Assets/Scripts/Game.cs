using System.Collections;
using Common;
using Enemies;
using Extensions;
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
    [SerializeField] private float _delayBeforePlayerRespawn = 1f;
        
    private void Awake()
    {
        _enemyHandler.Initialize();
        _gameUI.Initialize();
        _player.Initialize();

        _gameUI.OnStartWindowShowed += StartLevel;
        _gameUI.OnRestartButtonClicked += RestartLevel;

        _enemyHandler.OnAllEnemiesDied += WinGame;
        _enemyHandler.OnEnemyReachedBottomBorder += LoseGame;
        _enemyHandler.OnEnemyDied += EnemyDied;

        _player.OnTakeDamage += PlayerTakeDamage;
        _player.OnDied += LoseGame;
        
        _gameUI.ShowStartWindow();
    }

    private void StartLevel()
    {
        _enemyHandler.SpawnEnemies();
        
        _player.Activate();
        _player.StartMovement();
    }

    private void RestartLevel()
    {
        _enemyHandler.Reset();
        _player.SetMaxHealth();
        _gameUI.SetHealth(_player.Health);

        StartLevel();
    }

    private void WinGame()
    {
        _player.StopMovement();
            
        StartCoroutine(Helper.WaitCoroutine(_delayAfterWin, () =>
        {
            _player.IncreaseHealth();
            _gameUI.SetHealth(_player.Health);
            _enemyHandler.Reset();
            
            StartLevel();
        }));
    }

    private void LoseGame()
    {
        _player.StopMovement();
        _enemyHandler.StopMovement();

        StartCoroutine(Helper.WaitCoroutine(_delayAfterLose, () =>
        {
            _player.Deactivate();
            _enemyHandler.DespawnEnemies();
            _enemyHandler.Reset();
            _gameUI.ShowLoseWindow();
        }));
    }

    private void PlayerTakeDamage()
    {
        StartCoroutine(PlayerTakeDamageCoroutine());
    }
    
    private IEnumerator PlayerTakeDamageCoroutine()
    {
        _enemyHandler.StopMovement();

        yield return Helper.GetWait(_delayAfterPlayerDamage);
        
        _gameUI.SetHealth(_player.Health);
        _enemyHandler.StartMovement();
        _player.Deactivate();
        
        yield return Helper.GetWait(_delayBeforePlayerRespawn);
        
        _player.Respawn();
    }

    private void EnemyDied(EnemyData enemyData)
    {
        _gameUI.AddScore(enemyData.RewardScore);
    }

    private void OnDestroy()
    {
        _gameUI.OnStartWindowShowed -= StartLevel;
        _gameUI.OnRestartButtonClicked -= RestartLevel;

        _enemyHandler.OnEnemyReachedBottomBorder -= LoseGame;
        _enemyHandler.OnAllEnemiesDied -= WinGame;
        _enemyHandler.OnEnemyDied -= EnemyDied;

        _player.OnTakeDamage -= PlayerTakeDamage;
        _player.OnDied -= LoseGame;
    }
}