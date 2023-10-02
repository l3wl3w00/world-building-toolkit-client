#nullable enable
using System.Collections.Generic;
using Game.Linq;
using Game.Util;
using Generated;
using Unity.VisualScripting;
using UnityEngine;
using PrefabUtility = Common.PrefabUtility;

namespace Game.SceneChange
{
    public class SceneParametersBuilder
    {
        private readonly Dictionary<SceneParamKey, object> _values;

        public SceneParametersBuilder()
        {
            _values = new Dictionary<SceneParamKey, object>();
        }

        public SceneParametersBuilder(Dictionary<SceneParamKey, object> values)
        {
            _values = values;
        }

        public SceneParametersBuilder Add(SceneParamKey key, object value)
        {
            if (_values.ContainsKey(key)) _values[key] = value;
            _values.Add(key, value);
            return this;
        }

        public void Save()
        {
            Object.FindObjectOfType<SceneChangeParameters>().ToOption()
                .DoIfNotNull(AddValuesTo)
                .DoIfNull(() =>
                {
                    var prefab = PrefabUtility.Load(Prefab.SceneChangeParameters);
                    var parameters = Object.Instantiate(prefab).GetComponent<SceneChangeParameters>();
                    AddValuesTo(parameters);
                    Object.DontDestroyOnLoad(parameters);
                });

            void AddValuesTo(SceneChangeParameters parameters)
            {
                _values.ForEach(pair => parameters.Values.Add(pair.Key, pair.Value));
            }
        }
    }
}