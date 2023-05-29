using UnityEngine;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(menuName = "Border Data", fileName = "New BorderData")]
    public class BorderData : ScriptableObject
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;
        
        public float MinX => _minX;
        public float MaxX => _maxX;
        public float MinY => _minY;
        public float MaxY => _maxY;
    }
}