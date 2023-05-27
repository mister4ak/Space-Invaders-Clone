using ScriptableObjects.Classes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private BorderData _borderData;
    [SerializeField] private float _movementSpeed = 5f;

    private PlayerMover _mover;

    private void Awake()
    {
        var movementInput = new KeyboardMovementInput(false);
        _mover = new PlayerMover(movementInput, transform, _movementSpeed, _borderData);
        
        _weapon.Initialize();
    }

    private void Update()
    {
        _mover.Move();
        
        if (Input.GetKeyDown(KeyCode.Space))
            Shot();
    }

    private void Shot()
    {
        _weapon.TryShot();
    }
}