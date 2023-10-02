#nullable enable
using System.Collections.Generic;
using Game.SceneChange;
using Generated;
using UnityEngine.SceneManagement;

namespace Common
{
    public static class SceneUtility
    {
        public static void Load(this Scenes scene)
        {
            SceneManager.LoadScene(scene.Name);
        }

        public static void Load(this Scenes scene, Dictionary<SceneParamKey, object> parameters)
        {
            new SceneParametersBuilder(parameters).Save();
            SceneManager.LoadScene(scene.Name);
        }

        public static void Load(this Scenes scene, SceneParamKey paramName, object paramValue)
        {
            scene.Load(new Dictionary<SceneParamKey, object>
            {
                { paramName, paramValue }
            });
        }
    }
}