#nullable enable
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Util
{
    public static class NullChecker
    {
        public static void AssertNoneIsNullInType(Type type, params object?[] ts)
        {
            for (var index = 0; index < ts.Length; index++)
            {
                if (ts[index] != null) continue;
                
                Debug.LogError($"object passed to AssertNoneIsNull was null at position {index}. " +
                               $"Scene: {SceneManager.GetActiveScene().name}. " +
                               $"Class: {type.Name}. ");
                throw new NullReferenceException($"object passed to AssertNoneIsNull was null at position {index}");
            }
        }
    }
}