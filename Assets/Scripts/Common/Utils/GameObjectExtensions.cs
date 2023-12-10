#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.Utils
{
    public static class GameObjectExtensions
    {
        public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
        {
            var gameObjectTransform = gameObject.transform;
            for (var i = 0; i < gameObjectTransform.childCount; i++)
                yield return gameObjectTransform.GetChild(i).gameObject;
        }
        
        public static Option<GameObject> GetChild(this GameObject gameObject, string name)
        {
            return gameObject.GetChildren().FirstOrDefault(g => g.name == name).ToOption();
        }
        
        public static uint GetSelfIndexInParent(this GameObject gameObject) => GetSelfIndexInParent(gameObject.transform);

        public static uint GetSelfIndexInParent(this Transform transform)
        {
            var parent = transform.parent;
            for (uint i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i.ToInt()) == transform)
                {
                    return i;
                }
            }

            var message = $"{transform.name} was not found in the children of its own parent";
            Debug.Log(message, transform);
            throw new Exception(message);
        }
        public static Option<T> FindFirstParentOfType<T>(this GameObject gameObject) 
            where T : Component =>
            gameObject.transform.FindFirstParentOfType<T>();

        public static Option<T> FindFirstParentOfType<T>(this Transform transform) where T : Component
        {
            var parentOpt = transform.parent.ToOption();
            while (parentOpt.HasValueOut(out var parent))
            {
                var componentOpt = parent!.GetComponent<T>().ToOption();
                if (componentOpt.HasValue)
                {
                    return componentOpt;
                }

                parentOpt = parent.parent.ToOption();
            }
            
            return Option<T>.None;
        }

        public static Option<Transform> FindFirstParentOfName(this Transform transform, string name)
        {
            var parentOpt = transform.parent.ToOption();
            while (parentOpt.HasValueOut(out var parent))
            {
                if (parent!.name == name) return parent.ToOption();

                parentOpt = parent.parent.ToOption();
            }
            
            return Option<Transform>.None;
        }
        
        public static Option<GameObject> FindFirstParentOfName(this GameObject gameObject, string name)
        {
            return gameObject.transform.FindFirstParentOfName(name).NullOr(t => t.gameObject);
        }
        
        public static TComponent AssertComponentInChildren<TComponent>(this Component callerComponent) 
            where TComponent : Component
        {
            return callerComponent.GetComponentInChildren<TComponent>()
                .ToOption()
                .ExpectNotNull($"Component '{typeof(TComponent).Name}' was not found in the children of this game object, but it was expected to be");
        }
        
        public static int ToInt(this uint value)
        {
            return (int)value;
        }
        
        public static T Apply<T>(this T t, Action<T> action)
        {
            action(t);
            return t;
        }
        
        public static string GetPath(this GameObject gameObject)
        {
            var path = "/" + gameObject.name;
            while (gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;
                path = "/" + gameObject.name + path;
            }
            return path;
        }
    }
    
}