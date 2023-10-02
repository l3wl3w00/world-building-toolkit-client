#nullable enable
using System;
using Game.Client;
using Game.Constants;
using UI.Common.Button;
using UnityEngine;

namespace UI.WorldList
{
    public class DeleteWorldButton : ButtonControl<NoButtonParams>
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
            StartCoroutine(_client.DeleteWorld(Id, () => { Destroy(transform.parent.gameObject); }));
        }
    }
}