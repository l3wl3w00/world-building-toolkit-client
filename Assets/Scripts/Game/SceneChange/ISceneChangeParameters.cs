#nullable enable
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
                var parameters = Object.FindObjectOfType<SceneChangeParameters>();
                if (parameters == null)
                {
                    Debug.LogWarning("No SceneChangeParameters found in the scene");
                    return new EmptySceneChangeParameters();
                }

                return parameters;
            }
        }

        void Add(SceneParamKey key, object value);
        T Get<T>(SceneParamKey key);
        void Destroy();
    }
}