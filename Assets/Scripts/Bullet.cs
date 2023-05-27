using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        private float _speed;
        private Vector2 _direction;

        public void Initialize(Vector2 direction, float speed)
        {
            _direction = direction;
            _speed = speed;
        }

        public void Update()
        {
            transform.Translate(_speed * _direction * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage();
            }
            Destroy(gameObject);
        }
    }
}