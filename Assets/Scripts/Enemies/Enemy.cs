using System;
using Common.ObjectPool;
using Enums;
using Interfaces;
using ScriptableObjects.Classes;
using UnityEngine;
using VFX;
using Weapons;

namespace Enemies
{
    public class Enemy : PoolItem, IDamageable
    {
        public event Action<Enemy> Died;

        [SerializeField] private Weapon _weapon;
        [SerializeField] private EnemyData _data;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private ExplosionFX _explosionFXPrefab;

        public ShooterType ShooterType { get; private set; }
        public EnemyData Data => _data;

        public void Initialize()
        {
            ShooterType = ShooterType.Enemy;
            _weapon.Initialize(ShooterType);
            _enemyAnimator.Initialize(_data);
        }

        public void Move(float deltaX, float deltaY)
        {
            _enemyAnimator.Animate();
            transform.position += new Vector3(deltaX, deltaY);
        }

        public void TakeDamage()
        {
            Pool.Get(_explosionFXPrefab, transform.position);
            Died?.Invoke(this);
            Release();
        }

        public void Shot()
        {
            _weapon.TryShot();
        }
    }
}