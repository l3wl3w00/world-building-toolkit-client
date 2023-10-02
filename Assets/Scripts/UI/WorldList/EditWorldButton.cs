#nullable enable
using System;
using Common;
using Game.Client;
using Game.Constants;
using Game.SceneChange;
using Generated;
using UI.Common.Button;
using UnityEngine;

namespace UI.WorldList
{
    public class EditWorldButton : ButtonControl<NoButtonParams>
    {
        private WorldBuildingApiClient _client;

        #region Properties

        public Guid Id { private get; set; }

        #endregion

        protected override void OnStart()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
        }

        protected override void OnClickedTypesafe(NoButtonParams param)
        {
            Scenes.PlanetEditingLoadScene.Load(SceneParamKey.WorldId, Id);
        }
    }
}