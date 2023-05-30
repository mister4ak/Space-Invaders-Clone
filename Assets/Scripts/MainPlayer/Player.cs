using System;
using Enums;
using Extensions;
using Interfaces;
using MainPlayer.Movement;
using ScriptableObjects.Classes;
using UnityEngine;
using Weapons;
using Vector2 = UnityEngine.Vector2;

namespace MainPlayer
{
    public class Player : MonoBehaviour, IDamageable
    {
        public event Action OnTakeDamage;
        public event Action OnDied;
        
        [SerializeField] private Weapon _weapon;
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private BorderData _borderData;
        [SerializeField] private PlayerData _playerData;
    
        private PlayerMover _mover;
        private int _health;
        private bool _isMoving;

        public ShooterType ShooterType { get; private set; }
        public int Health => _health;

        public void Initialize()
        {
            gameObject.Deactivate();
        
            ShooterType = ShooterType.Player;
        
            var movementInput = new KeyboardMovementInput(false);
            _mover = new PlayerMover(movementInput, transform, _playerData.MovementSpeed, _borderData);

            _weapon.Initialize(ShooterType);

            SetMaxHealth();
        }

        private void Update()
        {
            if (!_isMoving) return;
        
            _mover.Move();
            HandleShoot();
        }

        public void SetMaxHealth()
        {
            _health = _playerData.MaxHealth;
        }

        public void TakeDamage()
        {
            _health--;
            StopMovement();
            _animator.PlayTakeDamage();

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

        public void StopMovement() => _isMoving = false;

        public void StartMovement() => _isMoving = true;

        public void Respawn()
        {
            gameObject.Activate();
        
            _mover.Warp(new Vector2(_borderData.MinX, _borderData.MinY));
            StartMovement();
        }

        private void HandleShoot()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Shot();
        }

        private void Shot()
        {
            _weapon.TryShot();
        }
    }
}