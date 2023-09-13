using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder.Client.Common
{
    public static class GameObjectExtensions
    {
        public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
        {
            var gameObjectTransform = gameObject.transform;
            for (int i = 0; i < gameObjectTransform.childCount; i++)
            {
                yield return gameObjectTransform.GetChild(i).gameObject;
            }
        }
    }
}