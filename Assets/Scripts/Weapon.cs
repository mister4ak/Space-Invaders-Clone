using System;
using Common.ObjectPool;
using DefaultNamespace;
using UnityEngine;

public enum ShootDirectionType
{
    Up = 0,
    Down = 1,
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private ShootDirectionType _shootDirectionType;
    [SerializeField] private float _speed;

    private ShooterType _shooterType;
    private bool _isReloaded;

    public void Initialize(ShooterType shooterType)
    {
        _shooterType = shooterType;
        _isReloaded = true;
    }

    public void TryShot()
    {
        if (!_isReloaded) return;
        _isReloaded = false;
            
        SpawnBullet();
    }

    private void SpawnBullet()
    {
        var bullet = Pool.Get(_bulletPrefab, _shotPoint.position);
        bullet.Initialize(_shooterType, GetShootDirection(), _speed);
        bullet.OnRelease += OnBulletRelease;
    }

    private Vector2 GetShootDirection()
    {
        return _shootDirectionType switch
        {
            ShootDirectionType.Up => Vector2.up,
            ShootDirectionType.Down => Vector2.down,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void OnBulletRelease(PoolItem bulletPoolItem)
    {
        bulletPoolItem.OnRelease -= OnBulletRelease;
        _isReloaded = true;
    }
}