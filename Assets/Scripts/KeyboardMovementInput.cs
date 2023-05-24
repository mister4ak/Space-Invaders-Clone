using UnityEngine;

public class KeyboardMovementInput : IMovementInput
{
    public Vector2 GetInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        return new Vector2(moveHorizontal, moveVertical);
    }
}