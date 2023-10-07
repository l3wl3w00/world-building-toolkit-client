#nullable enable
using Game.Util;
using UnityEngine;

namespace Game.SceneChange
{
    public class EmptySceneChangeParameters : ISceneChangeParameters
    {
        public void Add(SceneParamKey key, object value)
        {
        }

        public void Destroy()
        {
        }

        public Option<T> Get<T>(SceneParamKey key)
        {
            return Option<T>.None;
        }
    }
}