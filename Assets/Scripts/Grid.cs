using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private List<Cell> _cells;
    [SerializeField] private int _rowsCount;
    [SerializeField] private int _columnsCount;
    [SerializeField] private float _xPadding = 1f;
    [SerializeField] private float _yPadding = 1f;
    [SerializeField] private bool _showGizmo;

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
        float y = ((_rowsCount - 1) / 2f - i / _columnsCount) * _yPadding;
        var position = new Vector2(x, y);
        return position;
    }
    
    private void OnDrawGizmos()
    {
        if (!_showGizmo) return;
            
        for (var i = 0; i < _cells.Count; i++) 
            Gizmos.DrawSphere(transform.position + (Vector3) GetPosition(i), 0.3f);
    }
}