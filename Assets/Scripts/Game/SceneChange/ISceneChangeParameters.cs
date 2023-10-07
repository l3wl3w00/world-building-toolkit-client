#nullable enable
using Game.Util;
using UnityEngine;

namespace Game.SceneChange
{
    public enum SceneParamKey
    {
        WorldSummary,
        WorldId,
        WorldDetailed,
        InitialScreen
    }

    public interface ISceneChangeParameters
    {
        public static ISceneChangeParameters Instance
        {
            get
            {
                ISceneChangeParameters parameters = Object.FindObjectOfType<SceneChangeParameters>();
                return parameters.ToOption()
                    .LogWarnIfNull("No SceneChangeParameters found in the scene")
                    .ValueOr(new EmptySceneChangeParameters());
            }
        }

        void Add(SceneParamKey key, object value);
        Option<T> Get<T>(SceneParamKey key);

        T GetNonNullable<T>(SceneParamKey key)
        {
            return Get<T>(key)
                .DoIfNull(() => Debug.LogError($"No scene change parameter was found with key {key}")).Value;
        }
        void Destroy();
    }
}