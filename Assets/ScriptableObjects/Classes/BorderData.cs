using UnityEngine;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(menuName = "Border Data", fileName = "New BorderData")]
    public class BorderData : ScriptableObject
    {
        [field: SerializeField] public float MinX {get; private set;}
        [field: SerializeField] public float MaxX {get; private set;}
        [field: SerializeField] public float MinY {get; private set;}
        [field: SerializeField] public float MaxY {get; private set;}
    }
}