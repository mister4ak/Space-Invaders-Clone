using System;
using System.Collections.Generic;
using System.Linq;
using Common.ObjectPool;
using Enums;
using UnityEngine;

namespace Enemies
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemyPrefabs;

        public Enemy Create(EnemyType enemyType, Vector3 spawnPosition)
        {
            var enemyPrefab = _enemyPrefabs.FirstOrDefault(prefab => prefab.Data.Type == enemyType);
            if (enemyPrefab == default)
                throw new InvalidOperationException("Can't find enemy prefab with needed EnemyType");
            
            var enemy = Pool.Get(enemyPrefab, spawnPosition);
            enemy.Initialize();
            return enemy;
        }
    }
}