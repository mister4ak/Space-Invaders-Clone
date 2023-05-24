using UnityEngine;

public class PositionClamper : IPositionClamper
{
    private readonly float _minX;
    private readonly float _maxX;
    private readonly float _minY;
    private readonly float _maxY;

    public PositionClamper(float minX, float maxX, float minY, float maxY)
    {
        _minX = minX;
        _maxX = maxX;
        _minY = minY;
        _maxY = maxY;
    }

    public Vector3 ClampPosition(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, _minX, _maxX);
        float clampedY = Mathf.Clamp(position.y, _minY, _maxY);
        return new Vector3(clampedX, clampedY, position.z);
    }
}