#nullable enable
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
    }
}