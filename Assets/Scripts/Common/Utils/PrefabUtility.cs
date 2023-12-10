#nullable enable
using System.Collections.Generic;
using UnityEngine;
using Prefab = Common.Generated.Prefab;

namespace Common.Utils
{
    internal class PrefabLoader
    {
        private readonly Dictionary<Prefab, GameObject> _loadedPrefabs = new();

        public GameObject Get(Prefab prefab)
        {
            var prefabGameObject = _loadedPrefabs.GetValueOrDefault(prefab).ToOption();
            if (prefabGameObject.HasValue) return prefabGameObject.Value;

            var newlyLoadedPrefab = Resources.Load<GameObject>(prefab.Path)
                .ToOption()
                .DoIfNull(() => Debug.LogError($"Failed to load prefab {prefab.Path}!"))
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
        
        public static T InstantiateAndExpectComponent<T>(this Prefab prefabName)
            where T : Component
        {
            return Instantiate(prefabName, Option<Transform>.None).GetComponent<T>()
                .ToOption()
                .ExpectNotNull($"Component {typeof(T).Name} was expected to be on prefab '{prefabName.Name}', but it was not present");
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