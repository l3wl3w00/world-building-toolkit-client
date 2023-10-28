#nullable enable
using System;
using System.Collections.Generic;
using Common.SceneChange;
using Generated;
using UnityEngine.SceneManagement;

namespace Common.Utils
{
    public static class SceneUtility
    {
        public static void Load(this Scenes scene)
        {
            SceneManager.LoadScene(scene.Name);
        }

        public static void Load(this Scenes scene, Dictionary<Guid, object> parameters)
        {
            new SceneParametersBuilder(parameters).Save();
            SceneManager.LoadScene(scene.Name);
        }
        
        public static void Load<T>(this Scenes scene, SceneParamKey<T> paramName, T paramValue) where T : notnull
        {
            scene.Load(new Dictionary<Guid, object>
            {
                { paramName.Id, paramValue }
            });
        }
    }
}