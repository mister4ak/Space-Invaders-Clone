using System;
using System.Collections.Generic;
using System.Linq;
using Common.ObjectPool;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MoveDirectionType
{
    Left = 0,
    Right = 1,
    Down = 2,
}

public class Grid : MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private List<Cell> _cells;
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private int _rowsCount;
    [SerializeField] private int _columnsCount;
    [SerializeField] private float _xPadding = 1f;
    [SerializeField] private float _yPadding = 1f;
    [SerializeField] private bool _showGizmo;
    [SerializeField] private float _xDelta = 0.25f;
    [SerializeField] private float _yDelta = -0.25f;
    [SerializeField] private float _leftBorder = -8f;
    [SerializeField] private float _rightBorder = 8f;
    [SerializeField] private float _updateTimer = 0.025f;
    [SerializeField] private Vector2 _shootCooldownRange = new Vector2(1f, 10f);

    private float _timer;
    private float _shootTimer;
    private bool _isBorderReached;
    private MoveDirectionType _moveDirectionType;
    private MoveDirectionType _previousMoveDirectionType;
    private int _currentCellIndex;
    private readonly List<Enemy> _enemies = new();

    #region Create Grid Editor

    public void CreateGrid()
    {
        transform.DestroyChildren(true);
        _cells.Clear();
        
        for (int rowIndex = 0; rowIndex < _rowsCount; rowIndex++)
        {
            var rowContainer = CreateRowContainer(rowIndex);

            for (int columnIndex = 0; columnIndex < _columnsCount; columnIndex++)
            {
                var point = Instantiate(_cellPrefab, 
                    transform.position + (Vector3) GetPosition(rowIndex * _columnsCount + columnIndex), 
                    Quaternion.identity, 
                    rowContainer);
                _cells.Add(point);
            }
        }
    }

    private Transform CreateRowContainer(int rowIndex)
    {
        Transform rowContainer = new GameObject($"Row_{rowIndex + 1}").transform;
        rowContainer.SetParent(transform);
        rowContainer.localPosition = Vector3.zero;
        return rowContainer;
    }
    
    private Vector2 GetPosition(int i)
    {
        float x = (i % _columnsCount - (_columnsCount - 1) / 2f) * _xPadding;
        float y = (-(_rowsCount - 1) / 2f + i / _columnsCount) * _yPadding;
        var position = new Vector2(x, y);
        return position;
    }

    #endregion

    private void Start()
    {
        _moveDirectionType = MoveDirectionType.Left;
        _currentCellIndex = 0;
        SetRandomShootTime();
        foreach (var cell in _cells)
        {
            var enemyPrefab = _enemyPrefabs.FirstOrDefault(prefab => prefab.Type == cell.EnemyType);
            if (enemyPrefab == default)
                throw new InvalidOperationException("Can't find enemy prefab with needed EnemyType");
            
            var enemy = Pool.Get(enemyPrefab, cell.transform.position);
            enemy.Initialize();
            enemy.Died += OnEnemyDied;
            _enemies.Add(enemy);
        }
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.Died -= OnEnemyDied;
        if (_currentCellIndex > _enemies.IndexOf(enemy) || _currentCellIndex == _enemies.Count)
            _currentCellIndex--;
        _enemies.Remove(enemy);
    }

    private void Update()
    {
        HandleEnemyShot();
        HandleMove();
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
        _shootTimer = Random.Range(_shootCooldownRange.x, _shootCooldownRange.y);
    }

    private void HandleMove()
    {
        _timer += Time.deltaTime;

        if (_timer < _updateTimer) return;

        _timer = 0f;

        switch (_moveDirectionType)
        {
            case MoveDirectionType.Left:
                MoveEnemy(-_xDelta, 0f);
                CheckBorder();
                break;
            case MoveDirectionType.Right:
                MoveEnemy(_xDelta, 0f);
                CheckBorder();
                break;
            case MoveDirectionType.Down:
                MoveEnemy(0f, _yDelta);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        IncreaseCellIndex();
    }

    private void CheckBorder()
    {
        if (_isBorderReached) return;
        
        var cellPosition = _enemies[_currentCellIndex].transform.position;
        if (cellPosition.x <= _leftBorder || cellPosition.x >= _rightBorder)
            _isBorderReached = true;
    }

    private void MoveEnemy(float deltaX, float deltaY)
    {
        _enemies[_currentCellIndex].Move(deltaX, deltaY);
    }

    private void IncreaseCellIndex()
    {
        _currentCellIndex++;
        if (_currentCellIndex < _enemies.Count) return;
        
        ResetCellIndex();
        CalculateNextMoveDirection();
    }

    private void CalculateNextMoveDirection()
    {
        if (_moveDirectionType == MoveDirectionType.Down)
        {
            _moveDirectionType = _previousMoveDirectionType == MoveDirectionType.Left
                ? MoveDirectionType.Right
                : MoveDirectionType.Left;
            
            _previousMoveDirectionType = MoveDirectionType.Down;
            _isBorderReached = false;
        }
        else if (_isBorderReached)
        {
            _previousMoveDirectionType = _moveDirectionType;
            _moveDirectionType = MoveDirectionType.Down;
        }
    }

    private void ResetCellIndex()
    {
        _currentCellIndex = 0;
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmo) return;
            
        for (var i = 0; i < _rowsCount * _columnsCount; i++) 
            Gizmos.DrawSphere(transform.position + (Vector3) GetPosition(i), 0.3f);
    }
}