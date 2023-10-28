#nullable enable
using UnityEngine;

namespace Common.Utils
{
    public static class InitializeUtils
    {
        public static T LazyInitialize<T>(this Component thisComponent, ref Option<T> component) where T : Component
        {
            if (component.HasValue) return component.Value;
            
            component = thisComponent.GetComponent<T>().ToOption();
            component.DoIfNull(() => Debug.LogError($"No component {typeof(T).Name} was found, " +
                                                    $"but it was expected to be on the same game object as {thisComponent.GetType().Name}"));
            return component.Value;
        }
    }
}