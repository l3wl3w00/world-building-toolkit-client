#nullable enable
using System;
using Common;
using Game.Client;
using Game.Constants;
using Game.SceneChange;
using Game.Util;
using Generated;
using UI.Common.Button;
using UnityEngine;

namespace UI.WorldList
{
    public class EditWorldButton : ButtonControl<NoButtonParams>
    {
        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;

        #region Properties

        public Guid Id { private get; set; }

        #endregion

        protected override void OnStart()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey)).ToOption();
        }

        protected override void OnClickedTypesafe(NoButtonParams param)
        {
            Scenes.PlanetEditingLoadScene.Load(SceneParamKey.WorldId, Id);
        }
    }
}