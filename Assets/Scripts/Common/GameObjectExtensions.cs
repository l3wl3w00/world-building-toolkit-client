#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class GameObjectExtensions
    {
        public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
        {
            var gameObjectTransform = gameObject.transform;
            for (var i = 0; i < gameObjectTransform.childCount; i++)
                yield return gameObjectTransform.GetChild(i).gameObject;
        }
    }
}