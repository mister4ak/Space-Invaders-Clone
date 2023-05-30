using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Enemies
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private List<Cell> _cells;
        [SerializeField] private int _rowsCount = 5;
        [SerializeField] private int _columnsCount = 11;
        [SerializeField] private float _xPadding = 1f;
        [SerializeField] private float _yPadding = 1f;
        [SerializeField] private bool _showGizmo;
    
        public List<Cell> Cells => _cells;

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

        private void OnDrawGizmos()
        {
            if (!_showGizmo) return;
            
            for (var i = 0; i < _rowsCount * _columnsCount; i++) 
                Gizmos.DrawSphere(transform.position + (Vector3) GetPosition(i), 0.3f);
        }
    }
}