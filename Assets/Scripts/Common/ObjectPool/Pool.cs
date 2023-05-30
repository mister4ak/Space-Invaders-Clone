using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.ObjectPool
{
	public class Pool : MonoBehaviour
	{
		private static GameObject PoolGameObject { get; set; }
		private static Pool _instance;

		private static readonly Dictionary<int, Queue<IPoolable>> PoolItems  = new();
		private static readonly Dictionary<int, Transform>        Containers = new();
		private static readonly HashSet<IPoolable>                UsedItems  = new();
		private Transform CachedTransform => _cachedTransform == default ? _cachedTransform = transform : _cachedTransform;

		private Transform _cachedTransform;

		private void Awake()
		{
			if (_instance != default) Destroy(this);

			_instance = this;
			PoolGameObject = gameObject;
		}

		private void OnDestroy()
		{
			PoolItems.Clear();
			Containers.Clear();
			UsedItems.Clear();
		}

		public static Pool Instance
		{
			get
			{
				if (_instance != default) return _instance;

				PoolGameObject = new GameObject("###_MAIN_POOL_###");
				_instance = PoolGameObject.AddComponent<Pool>();

				return _instance;
			}
		}

		public static T Get<T>(T prefab, Vector3 position = default, Transform parent = default)
			where T : Object, IPoolable
		{
			T   pooledItem;
			var id        = prefab.GetInstanceID();
			var queue     = GetQueue(id);
			var container = GetContainer(id);
			if (queue.Count > 0)
			{
				pooledItem = (T) queue.Dequeue();
				var pooledItemTransform = pooledItem.MyTransform();
				if (parent != default) pooledItemTransform.parent = parent;

				pooledItemTransform.position = position;
				pooledItem.MyTransform().gameObject.SetActive(true);
				pooledItem.Restart();
				UsedItems.Add(pooledItem);

				UpdateContainerName(container, queue.Count, prefab.name);
				return pooledItem;
			}

			var newParent = parent == default ? container : parent;
			pooledItem = InstantiateObject(prefab, position, newParent, id);

			UpdateContainerName(container, 0, prefab.name);
			return pooledItem;
		}

		public static void Release<T>(int id, T poolItem, bool disableObject = true) where T : Object, IPoolable
		{
			var queue = GetQueue(id);
			if (!queue.Contains(poolItem)) queue.Enqueue(poolItem);
			UsedItems.Remove(poolItem);

			var container = GetContainer(id);
			poolItem.SetParent(container);
			UpdateContainerName(container, queue.Count, poolItem.ContainerName);

			if (disableObject) { poolItem.SetActive(false); }
		}

		public static void ReleaseAll()
		{
			foreach (var item in UsedItems.ToList()) { item.Release(); }
		}

		public static void ReleaseAll(PoolItem item) => ReleaseAll(item.GetInstanceID());

		public static void ReleaseAll(int id)
		{
			foreach (var item in UsedItems.Where(x => x.ID == id).ToList()) { item.Release(); }
		}

		private static T InstantiateObject<T>(T prefab, Vector3 position, Transform newParent, int id, bool activate = true)
			where T : UnityEngine.Object, IPoolable
		{
			var instance = Instantiate(prefab, position, prefab.MyTransform().rotation, newParent);
			instance.name = prefab.name;
			instance.Retain(id, prefab.name);
			instance.SetActive(activate);
			UsedItems.Add(instance);

			return instance;
		}

		private static Queue<IPoolable> GetQueue(int id)
		{
			if (PoolItems.TryGetValue(id, out var queue)) return queue;

			queue = new Queue<IPoolable>();
			PoolItems.Add(id, queue);

			return queue;
		}

		private static Transform GetContainer(int id)
		{
			if (Containers.TryGetValue(id, out var container)) { return container; }

			container = new GameObject().transform;
			container.parent = Instance.CachedTransform;
			Containers.Add(id, container);

			return container;
		}

		private static void UpdateContainerName(Transform container, int pooled, string name = default)
		{
#if UNITY_EDITOR
			var newName = name ?? container.name;
			if (name != default) container.name = $"{newName}\t{pooled}/{container.childCount}";
#endif
		}
	}
}