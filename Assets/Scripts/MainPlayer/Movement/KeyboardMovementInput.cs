using UnityEngine;

public class KeyboardMovementInput : IMovementInput
{
    private bool _isSmoothed;

    public KeyboardMovementInput(bool isSmoothed)
    {
        _isSmoothed = isSmoothed;
    }

    public Vector2 GetInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        return _isSmoothed ? new Vector2(moveHorizontal, moveVertical) : new Vector2(moveHorizontal, moveVertical).normalized;
    }
}