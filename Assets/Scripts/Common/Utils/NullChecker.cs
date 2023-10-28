#nullable enable
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Utils
{
    public static class NullChecker
    {
        public static void AssertNoneIsNullInType<T1>(Type type, T1 t1) => 
            AssertNoneIsNullInType(type, t1.ToObjectInfo());
        public static void AssertNoneIsNullInType<T1, T2>(Type type, T1 t1, T2 t2) => 
            AssertNoneIsNullInType(type,
                t1.ToObjectInfo(),
                t2.ToObjectInfo());
        public static void AssertNoneIsNullInType<T1, T2, T3>(Type type, T1 t1, T2 t2, T3 t3) => 
            AssertNoneIsNullInType(type,
                t1.ToObjectInfo(),
                t2.ToObjectInfo(),
                t3.ToObjectInfo());
        public static void AssertNoneIsNullInType<T1, T2, T3, T4>(Type type, T1 t1, T2 t2, T3 t3, T4 t4) => 
            AssertNoneIsNullInType(type,
                t1.ToObjectInfo(),
                t2.ToObjectInfo(),
                t3.ToObjectInfo(),
                t4.ToObjectInfo());
        public static void AssertNoneIsNullInType<T1, T2, T3, T4, T5>(Type type, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) => 
            AssertNoneIsNullInType(type,
                t1.ToObjectInfo(),
                t2.ToObjectInfo(),
                t3.ToObjectInfo(),
                t4.ToObjectInfo(),
                t5.ToObjectInfo());

        
        // private static void AssertNoneIsNullInType(Type type, params object?[] ts)
        // {
        //     for (var index = 0; index < ts.Length; index++)
        //     {
        //         if (ts[index] != null) continue;
        //         
        //         Debug.LogError($"object passed to AssertNoneIsNull was null at position {index}. " +
        //                        $"Scene: {SceneManager.GetActiveScene().name}. " +
        //                        $"Type of object: {Type.GetType(ts[index])}. " +
        //                        $"Class: {type.Name}. ");
        //         throw new NullReferenceException($"object passed to AssertNoneIsNull was null at position {index}");
        //     }
        // }
        
        private static void AssertNoneIsNullInType(Type type, params ObjectInfo[] objectInfos)
        {
            for (var index = 0; index < objectInfos.Length; index++)
            {
                var objectInfo = objectInfos[index];
                if (objectInfo.IsNotNull) continue;
                
                Debug.LogError($"object passed to AssertNoneIsNull was null at position {index}. " +
                               $"Scene: {SceneManager.GetActiveScene().name}. " +
                               $"Type of object: {objectInfo.Type}. " +
                               $"Class: {type.Name}. ");
            }
        }

        private static ObjectInfo ToObjectInfo<T>(this T t)
        {
            return new ObjectInfo(t, typeof(T));
        }
        private struct ObjectInfo
        {
            public readonly object? Value;
            public readonly Type Type;

            public ObjectInfo(object? value, Type type)
            {
                this.Value = value;
                this.Type = type;
            }
            
            public bool IsNull => Value == null;
            public bool IsNotNull => !IsNull;
        }
    }
}