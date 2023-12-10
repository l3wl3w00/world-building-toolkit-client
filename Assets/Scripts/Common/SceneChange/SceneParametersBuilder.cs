#nullable enable
using System;
using System.Collections.Generic;
using Common.Utils;
using Unity.VisualScripting;
using Object = UnityEngine.Object;
using Prefab = Common.Generated.Prefab;

namespace Common.SceneChange
{
    public class SceneParametersBuilder
    {
        private readonly Dictionary<Guid, object> _values;

        public SceneParametersBuilder()
        {
            _values = new Dictionary<Guid, object>();
        }

        public SceneParametersBuilder(Dictionary<Guid, object> values)
        {
            _values = values;
        }

        public SceneParametersBuilder Add<T>(SceneParamKey<T> key, T value) where T : notnull
        {
            if (_values.ContainsKey(key.Id)) _values[key.Id] = value;
            _values.Add(key.Id, value);
            return this;
        }

        public void Save()
        {
            Object.FindObjectOfType<SceneChangeParameters>()
                .ToOption()
                .DoIfNotNull(AddValuesTo)
                .DoIfNull(() =>
                {
                    var parameters =
                        Prefab.SceneChangeParameters.InstantiateAndExpectComponent<SceneChangeParameters>();
                    AddValuesTo(parameters);
                    Object.DontDestroyOnLoad(parameters);
                });

            void AddValuesTo(SceneChangeParameters parameters)
            {
                foreach (var (key, value) in _values)
                {
                    parameters.Values.Add(key, value);
                }
            }
        }
    }
}