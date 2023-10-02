using System.Linq;
using Game.Client;
using Game.Client.Dto;
using Game.Constants;
using TMPro;
using UI.Common.Button;
using UnityEngine;

namespace Game.Hud.Button.Default
{
    public class CreateWorldButton : HudButtonControl<NoButtonParams>
    {
        private WorldBuildingApiClient _client;

        protected override void OnStart()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
        }

        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            var texts = FindObjectsOfType<TextMeshProUGUI>();
            var worldName = texts.Single(t => t.name == "NameInput").text.Replace("\u200b", "");
            var description = texts.Single(t => t.name == "DescriptionInput").text.Replace("\u200b", "");

            StartCoroutine(
                _client.CreateWorld(
                    new CreateWorldDto
                    {
                        name = worldName,
                        description = description
                    },
                    w =>
                    {
                        PlanetControl.Planet = new Planet.Planet(w.id, w.name, w.description);
                        HudController.CurrentHudScreen = HudScreen.PlanetEdit;
                        HudController.UpdateInputFields();
                    },
                    e => e.LogError()));
        }
    }
}