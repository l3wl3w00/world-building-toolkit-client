#nullable enable
using Game.Client;
using Game.Constants;
using UI.Common.Button;
using UnityEngine;

namespace UI.PlanetEdit.Button
{
    public class CreateContinentButton : ButtonControl<NoButtonParams>
    {
        private WorldBuildingApiClient _worldBuildingApiClient;

        #region Event Functions

        private void Start()
        {
            _worldBuildingApiClient = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
        }

        #endregion

        protected override void OnClickedTypesafe(NoButtonParams param)
        {
        }
    }
}