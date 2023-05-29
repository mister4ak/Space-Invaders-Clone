using UnityEngine;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(menuName = "Enemies Move Data", fileName = "New EnemiesMoveData")]
    public class EnemiesMoveData : ScriptableObject
    {
        [SerializeField] private float _xDelta = 0.25f;
        [SerializeField] private float _yDelta = -0.25f;
        [SerializeField] private float _leftBorder = -8f;
        [SerializeField] private float _rightBorder = 8f;
        [SerializeField] private float _bottomBorder = -3.5f;
        [SerializeField] private float _updateTimer = 0.025f;

        public float XDelta => _xDelta;

        public float YDelta => _yDelta;

        public float LeftBorder => _leftBorder;

        public float RightBorder => _rightBorder;

        public float BottomBorder => _bottomBorder;

        public float UpdateTimer => _updateTimer;
    }
}