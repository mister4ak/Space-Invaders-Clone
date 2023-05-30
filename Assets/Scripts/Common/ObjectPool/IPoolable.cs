using System;
using UnityEngine;

namespace Common.ObjectPool
{
    public interface IPoolable
    {
        int ID { get; }
        string ContainerName { get; }
        event Action OnRestart;
        event Action<PoolItem> OnRelease;
        Transform MyTransform();
        void Restart();
        void Retain(int id, string containerName);
        void Release(bool disableObject = true);
        void SetParent(Transform parent);
        void SetActive(bool active);
    }
}