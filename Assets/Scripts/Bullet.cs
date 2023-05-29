using Common.ObjectPool;
using UnityEngine;

public class Bullet : PoolItem
{
    [SerializeField] private ExplosionFX _explosionFXPrefab;
        
    private float _speed;
    private Vector2 _direction;
    private ShooterType _shooterType;

    public void Initialize(ShooterType shooterType, Vector2 direction, float speed)
    {
        _shooterType = shooterType;
        _direction = direction;
        _speed = speed;
    }

    public void Update()
    {
        transform.Translate(_speed * _direction * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            if (damageable.ShooterType == _shooterType) return;
                
            damageable.TakeDamage();
            Release();
        }
        else
        {
            Pool.Get(_explosionFXPrefab, transform.position);
            Release();
        }
    }
}