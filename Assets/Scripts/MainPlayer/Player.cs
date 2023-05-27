using MainPlayer.Movement;
using ScriptableObjects.Classes;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private BorderData _borderData;
    [SerializeField] private float _movementSpeed = 5f;

    private PlayerMover _mover;

    public ShooterType ShooterType { get; private set; }

    private void Awake()
    {
        var movementInput = new KeyboardMovementInput(false);
        _mover = new PlayerMover(movementInput, transform, _movementSpeed, _borderData);

        ShooterType = ShooterType.Player;
        _weapon.Initialize(ShooterType);
    }

    public void TakeDamage()
    {
        Debug.Log("Player take damage");
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