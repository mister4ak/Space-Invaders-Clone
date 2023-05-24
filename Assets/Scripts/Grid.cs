using System;
using System.Collections.Generic;
using UnityEngine;

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

    private float _timer;
    private bool _isBorderReached;
    private MoveDirectionType _moveDirectionType;
    private MoveDirectionType _previousMoveDirectionType;
    private int _currentCellIndex;

    public void CreateGrid()
    {
        foreach (var cell in _cells) 
            DestroyImmediate(cell.gameObject);
        _cells.Clear();
        
        for (int i = 0; i < _rowsCount * _columnsCount; i++)
        {
            var point = Instantiate(_cellPrefab, transform.position + (Vector3) GetPosition(i), Quaternion.identity, transform);
            _cells.Add(point);
        }
    }

    private Vector2 GetPosition(int i)
    {
        float x = (i % _columnsCount - (_columnsCount - 1) / 2f) * _xPadding;
        float y = (-(_rowsCount - 1) / 2f + i / _columnsCount) * _yPadding;
        var position = new Vector2(x, y);
        return position;
    }

    private void Start()
    {
        _moveDirectionType = MoveDirectionType.Left;
        _currentCellIndex = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _updateTimer) return;

        _timer = 0f;
        
        switch (_moveDirectionType)
        {
            case MoveDirectionType.Left:
                MoveCell(-_xDelta, 0f);
                CheckBorder();
                break;
            case MoveDirectionType.Right:
                MoveCell(_xDelta, 0f);
                CheckBorder();
                break;
            case MoveDirectionType.Down:
                MoveCell(0f, _yDelta);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        IncreaseCellIndex();
    }

    private void CheckBorder()
    {
        if (_isBorderReached) return;
        
        var cellPosition = _cells[_currentCellIndex].transform.position;
        if (cellPosition.x <= _leftBorder || cellPosition.x >= _rightBorder)
            _isBorderReached = true;
    }

    private void MoveCell(float deltaX, float deltaY)
    {
        _cells[_currentCellIndex].transform.position += new Vector3(deltaX, deltaY);
    }

    private void IncreaseCellIndex()
    {
        _currentCellIndex++;
        if (_currentCellIndex < _cells.Count) return;
        
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