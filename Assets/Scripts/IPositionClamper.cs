using UnityEngine;

namespace Scripts
{
    public interface IPositionClamper
    {
        Vector3 ClampPosition(Vector3 position);
    }
}