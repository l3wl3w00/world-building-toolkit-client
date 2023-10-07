#nullable enable
using Game.Util;
using UnityEngine;

namespace Game.Continent
{
    internal static class ContinentUtils
    {
        internal static T LazyInitialize<T>(this Component thisComponent, ref Option<T> component) where T : Component
        {
            if (component.HasValue) return component.Value;
            
            component = thisComponent.GetComponent<T>().ToOption();
            component.DoIfNull(() => Debug.LogError($"No component {nameof(T)} was found"));
            return component.Value;
        }
    }
}