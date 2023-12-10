#nullable enable
using System;
using System.Collections.Generic;
using Common.Generated;
using Common.SceneChange;
using UnityEngine.SceneManagement;
using Scene = Common.Generated.Scene;

namespace Common.Utils
{
    public static class SceneUtility
    {
        public static void Load(this Scene scene)
        {
            SceneManager.LoadScene(scene.Name);
        }

        public static void Load(this Scene scene, Dictionary<Guid, object> parameters)
        {
            new SceneParametersBuilder(parameters).Save();
            SceneManager.LoadScene(scene.Name);
        }
        
        public static void Load<T>(this Scene scene, SceneParamKey<T> paramName, T paramValue) where T : notnull
        {
            scene.Load(new Dictionary<Guid, object>
            {
                { paramName.Id, paramValue }
            });
        }
    }
}