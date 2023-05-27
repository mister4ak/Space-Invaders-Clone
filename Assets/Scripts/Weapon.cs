using UnityEngine;

namespace DefaultNamespace
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _shotPoint;
        [SerializeField] private float _speed;

        public void Shot()
        {
            var bullet = Instantiate(_bulletPrefab, _shotPoint.position, Quaternion.identity);
            bullet.Initialize(Vector2.up, _speed);
        }
    }
}