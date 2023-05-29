using System;
using Common.ObjectPool;
using ScriptableObjects.Classes;
using UnityEngine;

public class Enemy : PoolItem, IDamageable
{
    public event Action<Enemy> Died;

    [SerializeField] private Weapon _weapon;
    [SerializeField] private EnemyData _data;
    [SerializeField] private ExplosionFX _explosionFXPrefab;

    public ShooterType ShooterType { get; private set; }
    public EnemyType Type => _data.Type;

    public void Initialize()
    {
        ShooterType = ShooterType.Enemy;
        _weapon.Initialize(ShooterType);
    }

    public void Move(float deltaX, float deltaY)
    {
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