#nullable enable
using System.Collections.Generic;
using Game.Util;
using Generated;
using UnityEngine;

namespace Common
{
    internal class PrefabLoader
    {
        private readonly Dictionary<Prefab, GameObject> _loadedPrefabs = new();

        public GameObject Get(Prefab prefab)
        {
            var prefabGameObject = _loadedPrefabs.GetValueOrDefault(prefab).ToOption();
            if (prefabGameObject.HasValue) return prefabGameObject.Value;

            var newlyLoadedPrefab = (Resources.Load(prefab.Name) as GameObject).ToOption();
            newlyLoadedPrefab
                .DoIfNull(() => Debug.LogError($"Failed to load prefab {prefab.Name}!"))
                .DoIfNotNull(gameObject => _loadedPrefabs.Add(prefab, gameObject));
            return newlyLoadedPrefab.Value;
        }
    }

    public static class PrefabUtility
    {
        private static readonly PrefabLoader PrefabLoader = new();

        public static GameObject Load(this Prefab prefab)
        {
            return PrefabLoader.Get(prefab);
        }

        
        public static GameObject Instantiate(this Prefab prefabName)
        {
            return Instantiate(prefabName, Option<Transform>.None);
        }
        public static GameObject Instantiate(this Prefab prefabName, Option<Transform> parent)
        {
            return parent.Map(
                onHasValue: transform => Object.Instantiate(prefabName.Load(), transform),
                    onNull:        () => Object.Instantiate(prefabName.Load()));
        }

        // public static T Instantiate<T>(this Prefab prefabName) where T : Component
        // {
        //     return Object.Instantiate(prefabName.Load()).GetComponent<T>();
        // }
    }
}