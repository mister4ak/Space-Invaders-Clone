using System;
using System.Collections.Generic;
using ScriptableObjects.Classes;
using UnityEngine;

public enum MoveDirectionType
{
    Left = 0,
    Right = 1,
    Down = 2,
}

public class EnemiesMover
{
    public event Action OnBottomBorderReached;
    
    private readonly EnemiesMoveData _enemiesMoveData;
    private readonly List<Enemy> _enemies;
    
    private MoveDirectionType _moveDirectionType;
    private MoveDirectionType _previousMoveDirectionType;
    
    private float _timer;
    private int _currentCellIndex;
    private bool _isSideBorderReached;

    public EnemiesMover(List<Enemy> enemies, EnemiesMoveData enemiesMoveData)
    {
        _enemies = enemies;
        _enemiesMoveData = enemiesMoveData;
    }

    public void Move()
    {
        _timer += Time.deltaTime;

        if (_timer < _enemiesMoveData.UpdateTimer) 
            return;

        _timer = 0f;

        MoveInDirection();
        IncreaseCellIndex();
    }

    private void MoveInDirection()
    {
        switch (_moveDirectionType)
        {
            case MoveDirectionType.Left:
                MoveEnemy(-_enemiesMoveData.XDelta, 0f);
                CheckSideBorders();
                break;
            case MoveDirectionType.Right:
                MoveEnemy(_enemiesMoveData.XDelta, 0f);
                CheckSideBorders();
                break;
            case MoveDirectionType.Down:
                MoveEnemy(0f, _enemiesMoveData.YDelta);
                CheckBottomBorder();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void EnemyDied(Enemy enemy)
    {
        if (_currentCellIndex > _enemies.IndexOf(enemy) || _currentCellIndex == _enemies.Count - 1)
            _currentCellIndex--;
    }

    public void Reset()
    {
        _moveDirectionType = MoveDirectionType.Left;
        _previousMoveDirectionType = _moveDirectionType;
        _currentCellIndex = 0;
    }

    private void MoveEnemy(float deltaX, float deltaY)
    {
        _enemies[_currentCellIndex].Move(deltaX, deltaY);
    }

    private void CheckSideBorders()
    {
        if (_isSideBorderReached) 
            return;
        
        var enemyPosition = _enemies[_currentCellIndex].transform.position;
        if (IsOutsideBorders(enemyPosition))
            _isSideBorderReached = true;
    }

    private bool IsOutsideBorders(Vector3 enemyPosition)
    {
        return enemyPosition.x <= _enemiesMoveData.LeftBorder || enemyPosition.x >= _enemiesMoveData.RightBorder;
    }

    private void CheckBottomBorder()
    {
        var enemyPosition = _enemies[_currentCellIndex].transform.position;
        if (enemyPosition.y <= _enemiesMoveData.BottomBorder)
            OnBottomBorderReached?.Invoke();
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
            _isSideBorderReached = false;
        }
        else if (_isSideBorderReached)
        {
            _previousMoveDirectionType = _moveDirectionType;
            _moveDirectionType = MoveDirectionType.Down;
        }
    }

    private void ResetCellIndex() => _currentCellIndex = 0;
}