#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Game.SceneChange
{
    public class SceneChangeParameters : MonoBehaviour, ISceneChangeParameters
    {
        #region Properties

        public Dictionary<SceneParamKey, object> Values { get; } = new();

        #endregion

        #region Event Functions

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        #endregion

        #region ISceneChangeParameters Members

        public void Add(SceneParamKey key, object value)
        {
            if (Values.ContainsKey(key)) Values[key] = value;
            Values.Add(key, value);
        }

        public T Get<T>(SceneParamKey key)
        {
            return (T)Values[key];
        }

        public void Destroy()
        {
            Destroy(this);
        }

        #endregion
    }
}