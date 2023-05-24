using UnityEngine;

public class Player : MonoBehaviour
{
    private IMovementInput _movementInput;
    private IPositionClamper _positionClamper;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    private void Awake()
    {
        _movementInput = new KeyboardMovementInput();
        _positionClamper = new PositionClamper(_minX, _maxX, _minY, _maxY);
    }

    private void Update()
    {
        Vector2 input = _movementInput.GetInput();
        Vector3 movement = new Vector3(input.x, input.y) * _speed * Time.deltaTime;
        transform.position = _positionClamper.ClampPosition(transform.position + movement);
    }
}