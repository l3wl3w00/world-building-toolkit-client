#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.SceneChange
{
    public class SceneChangeParameters : MonoBehaviour
    {
        public Dictionary<Guid, object> Values { get; } = new();

        
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        
        public void Add<T>(SceneParamKey<T> key, T value) where T: notnull
        {
            if (Values.ContainsKey(key.Id)) Values[key.Id] = value;
            Values.Add(key.Id, value);
        }

        public Option<T> Get<T>(SceneParamKey<T> key) where T : notnull
        {
            return ((T)Values[key.Id]).ToOption();
        }
        
        public T GetNonNullable<T>(SceneParamKey<T> key) where T : notnull
        {
            return Get(key)
                .DoIfNull(() => Debug.LogError($"No scene change parameter was found with key {key}")).Value;
        }

        public void Destroy()
        {
            Destroy(this);
        }

        public static Option<SceneChangeParameters> Instance => FindObjectOfType<SceneChangeParameters>().ToOption();
        public static SceneChangeParameters NonNullInstance(Type type, string explanation = "") =>
            FindObjectOfType<SceneChangeParameters>()
                .ToOption()
                .ExpectNotNull($"No SceneChangeParameters in type {type.Name}, even though it was expected. " +
                               $"Explanation: {explanation}");
    }

    public class SceneParamKey<T>
    {
        public Guid Id { get; }
        public SceneParamKey()
        {
            Id = Guid.NewGuid();
        }
    }
}