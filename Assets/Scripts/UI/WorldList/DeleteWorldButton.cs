#nullable enable
using System;
using Game.Client;
using Game.Constants;
using Game.Util;
using UI.Common.Button;
using UnityEditor.UI;
using UnityEngine;

namespace UI.WorldList
{
    public class DeleteWorldButton : ButtonControl<NoButtonParams>
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
            _client
                .DoIfNotNull(c =>
                    StartCoroutine(c.DeleteWorld(Id, () => Destroy(transform.parent.gameObject))))
                .DoIfNull(() => Debug.LogError($"client was null in {this.name}"));
        }
    }
}