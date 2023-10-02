using Game.Client;
using Game.Client.Dto;
using Game.Constants;
using Game.Continent;
using UI.Common.Button;
using UnityEngine;

namespace Game.Hud.Button.Edit
{
    public class UpdateContinentButton : HudButtonControl<NoButtonParams>
    {
        private WorldBuildingApiClient _client;

        protected override void OnStart()
        {
            base.OnStart();
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
        }

        protected override void OnClickedTypesafe(NoButtonParams param)
        {
            PlanetControl.SelectedContinent
                .DoIfNotNull(UpdateContinent)
                .DoIfNull(() => Debug.LogError("Selected continent was null"));
        }

        private void UpdateContinent(ContinentHandler selectedContinent)
        {
            var dto = new PatchContinentDto(
                selectedContinent.Invert,
                selectedContinent.ContinentName,
                selectedContinent.ContinentDescription);

            StartCoroutine(_client.PatchContinent(
                selectedContinent.Id,
                dto,
                d =>
                {
                    HudController.UpdateInputFields();
                    Debug.Log("Successful update!");
                },
                e => { }));
        }
    }
}