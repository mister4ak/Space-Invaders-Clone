using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using ScriptableObjects.Classes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyHandler : MonoBehaviour
    {
        public event Action OnEnemyReachedBottomBorder;
        public event Action OnAllEnemiesDied;
        public event Action<EnemyData> OnEnemyDied;

        [Header("References")]
        [SerializeField] private Grid _grid;
        [SerializeField] private EnemyFactory _enemyFactory;
        [Header("Data")]
        [SerializeField] private EnemiesMoveData _enemiesMoveData;
        [SerializeField] private float _minShootCooldown = 1f;
        [SerializeField] private float _maxShootCooldown = 10f;

        private EnemiesMover _enemiesMover;
        private readonly List<Enemy> _enemies = new();
        private float _shootTimer;
        private bool _isMoving;

        public void Initialize()
        {
            _enemiesMover = new EnemiesMover(_enemies, _enemiesMoveData);
            _enemiesMover.OnBottomBorderReached += OnBottomBorderReached;
            ResetData();
        }

        public void SpawnEnemies()
        {
            StartCoroutine(SpawnEnemiesCoroutine());
        }

        public void StartMovement() => _isMoving = true;

        public void StopMovement() => _isMoving = false;

        public void ResetData()
        {
            SetRandomShootTime();
            _enemiesMover.ResetData();
        }

        public void DespawnEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Release();
                enemy.Died -= EnemyDied;
            }
            _enemies.Clear();
        }

        private void Update()
        {
            if (!_isMoving) return;
            _enemiesMover.Move();
            HandleEnemyShot();
        }

        private IEnumerator SpawnEnemiesCoroutine()
        {
            foreach (var cell in _grid.Cells)
            {
                var enemy = _enemyFactory.Create(cell.EnemyType, cell.transform.position);
                enemy.Died += EnemyDied;
                _enemies.Add(enemy);
                yield return null;
            }
            
            StartMovement();
        }

        private void OnBottomBorderReached()
        {
            OnEnemyReachedBottomBorder?.Invoke();
        }

        private void HandleEnemyShot()
        {
            _shootTimer -= Time.deltaTime;

            if (_shootTimer > 0) return;
        
            SetRandomShootTime();
            _enemies.GetRandomElement().Shot();
        }

        private void SetRandomShootTime()
        {
            _shootTimer = Random.Range(_minShootCooldown, _maxShootCooldown);
        }
        
        private void EnemyDied(Enemy enemy)
        {
            enemy.Died -= EnemyDied;
            _enemiesMover.EnemyDied(enemy);
            _enemies.Remove(enemy);
            OnEnemyDied?.Invoke(enemy.Data);

            if (_enemies.Count > 0) return;
            AllEnemiesDied();
        }

        private void AllEnemiesDied()
        {
            StopMovement();
            OnAllEnemiesDied?.Invoke();
        }

        private void OnDestroy()
        {
            _enemiesMover.OnBottomBorderReached -= OnBottomBorderReached;
        }
    }
}