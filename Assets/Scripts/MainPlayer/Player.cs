using System;
using System.Numerics;
using MainPlayer.Movement;
using ScriptableObjects.Classes;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour, IDamageable
{
    public event Action OnTakeDamage;
    public event Action OnDied;
        
    [SerializeField] private Weapon _weapon;
    [SerializeField] private BorderData _borderData;
    [SerializeField] private PlayerData _playerData;
    
    private PlayerMover _mover;
    private int _health;
    private bool _isMoving;

    public ShooterType ShooterType { get; private set; }
    public int Health => _health;

    private void Awake()
    {
        var movementInput = new KeyboardMovementInput(false);
        _mover = new PlayerMover(movementInput, transform, _playerData.MovementSpeed, _borderData);

        ShooterType = ShooterType.Player;
        _weapon.Initialize(ShooterType);
    }

    public void TakeDamage()
    {
        _health--;
        StopMovement();

        if (_health <= 0)
            OnDied?.Invoke();
        else
            OnTakeDamage?.Invoke();
    }

    public void IncreaseHealth()
    {
        if (_health < _playerData.MaxHealth)
            _health++;
    }

    private void Update()
    {
        if (!_isMoving) return;
        
        _mover.Move();
        
        if (Input.GetKeyDown(KeyCode.Space))
            Shot();
    }

    private void Shot()
    {
        _weapon.TryShot();
    }

    public void StopMovement()
    {
        _isMoving = false;
    }
    
    public void StartMovement()
    {
        _isMoving = true;
    }

    public void Respawn()
    {
        _mover.Warp(new Vector2(_borderData.MinX, _borderData.MinY));
        StartMovement();
    }
}