using System;
using UnityEngine;

namespace Common.ObjectPool
{
    public interface IPoolable
    {
        public int ID { get; }
        public string ContainerName { get; }
        public event Action OnRestart;
        public event Action<PoolItem> OnRelease;
        Transform MyTransform();
        public void Restart();
        public void Retain(int id, string containerName);
        public void Release(bool disableObject = true);
        public void SetParent(Transform parent);
        public void SetActive(bool active);
    }
}