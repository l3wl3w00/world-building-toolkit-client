#nullable enable
using UnityEngine;

namespace Game.Continent
{
    internal static class ContinentUtils
    {
        internal static T LazyInitialize<T>(this Component thisComponent, ref T? component) where T : Component
        {
            if (component != null) return component;
            component = thisComponent.GetComponent<T>();
            return component;
        }
    }
}