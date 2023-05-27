using System;
using Common.ObjectPool;
using UnityEngine;

public class Enemy : PoolItem, IDamageable
{
    public event Action<Enemy> Died;

    [SerializeField] private Weapon _weapon;
    [SerializeField] private EnemyType _type;

    public ShooterType ShooterType { get; private set; }
    public EnemyType Type => _type;

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
        Destroy(gameObject);
        Died?.Invoke(this);
    }

    public void Shot()
    {
        _weapon.TryShot();
    }
}