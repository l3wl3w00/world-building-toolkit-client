#nullable enable
using System;
using Common;
using Game.Client;
using Game.Constants;
using Game.Hud;
using Game.SceneChange;
using Generated;
using UnityEngine;

namespace Game
{
    public class IndividualWorldLoader : MonoBehaviour
    {
        #region Event Functions

        // Start is called before the first frame update
        private void Start()
        {
            var worldId = ISceneChangeParameters.Instance.Get<Guid>(SceneParamKey.WorldId);
            var client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
            StartCoroutine(client.GetWorld(worldId, worldDetailed =>
            {
                new SceneParametersBuilder()
                    .Add(SceneParamKey.WorldDetailed, worldDetailed)
                    .Add(SceneParamKey.InitialScreen, HudScreen.PlanetEdit)
                    .Save();
                Scenes.PlanetEditingScene.Load();
            }));
        }

        // Update is called once per frame
        private void Update()
        {
        }

        #endregion
    }
}