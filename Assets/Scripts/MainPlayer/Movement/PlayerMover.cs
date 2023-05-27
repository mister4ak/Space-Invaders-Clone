using ScriptableObjects.Classes;
using UnityEngine;

namespace MainPlayer.Movement
{
    public class PlayerMover
    {
        private readonly IMovementInput _movementInput;
        private readonly Transform _transform;
        private readonly float _movementSpeed;
        private readonly BorderData _borderData;

        public PlayerMover(IMovementInput movementInput, Transform transform, float movementSpeed, BorderData borderData)
        {
            _borderData = borderData;
            _movementSpeed = movementSpeed;
            _transform = transform;
            _movementInput = movementInput;
        }

        public void Move()
        {
            Vector2 input = _movementInput.GetInput();
            Vector3 movement = new Vector3(input.x, input.y) * _movementSpeed * Time.deltaTime;
            _transform.position = ClampPosition(_transform.position + movement);
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            float clampedX = Mathf.Clamp(position.x, _borderData.MinX, _borderData.MaxX);
            float clampedY = Mathf.Clamp(position.y, _borderData.MinY, _borderData.MaxY);
            return new Vector3(clampedX, clampedY, position.z);
        }
    }
}