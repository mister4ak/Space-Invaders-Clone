using Common.ObjectPool;
using DefaultNamespace;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _speed;

    private bool _isReloaded;
        
    public void Initialize()
    {
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
        bullet.Initialize(Vector2.up, _speed);
        bullet.OnRelease += OnBulletRelease;
    }

    private void OnBulletRelease(PoolItem bulletPoolItem)
    {
        bulletPoolItem.OnRelease -= OnBulletRelease;
        _isReloaded = true;
    }
}