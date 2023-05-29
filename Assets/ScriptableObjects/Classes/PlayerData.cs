using UnityEngine;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(menuName = "Player Data", fileName = "PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField, Range(2, 20)] private float _movementSpeed = 5f;
        [SerializeField, Min(1)] private int _maxHealth = 3;

        public float MovementSpeed => _movementSpeed;

        public int MaxHealth => _maxHealth;
    }
}