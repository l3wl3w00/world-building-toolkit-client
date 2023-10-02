#nullable enable
using System.Collections.Generic;
using Generated;
using UnityEngine;

namespace Common
{
    internal class PrefabLoader
    {
        private readonly Dictionary<Prefab, GameObject> _loadedPrefabs = new();

        public GameObject Get(Prefab prefab)
        {
            var contains = _loadedPrefabs.TryGetValue(prefab, out var prefabGameObject);
            if (contains) return prefabGameObject;

            prefabGameObject = Resources.Load(prefab.Name) as GameObject;
            _loadedPrefabs.Add(prefab, prefabGameObject);
            return prefabGameObject;
        }
    }

    public static class PrefabUtility
    {
        private static readonly PrefabLoader _prefabLoader = new();

        public static GameObject Load(this Prefab prefab)
        {
            return _prefabLoader.Get(prefab);
        }

        public static GameObject Instantiate(this Prefab prefabName, Transform parent = null)
        {
            if (parent == null) return Object.Instantiate(prefabName.Load());
            return Object.Instantiate(prefabName.Load(), parent);
        }

        public static T Instantiate<T>(this Prefab prefabName) where T : Component
        {
            return Object.Instantiate(prefabName.Load()).GetComponent<T>();
        }
    }
}