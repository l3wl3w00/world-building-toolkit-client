using System.Linq;
using Game.Client;
using Game.Client.Dto;
using Game.Constants;
using TMPro;
using UI.Common.Button;
using UnityEngine;

namespace Game.Hud.Button.Default
{
    public class UpdateWorldButton : HudButtonControl<NoButtonParams>
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
                _client.UpdateWorld(
                    PlanetControl.Planet.Id,
                    new CreateWorldDto
                    {
                        name = worldName,
                        description = description
                    },
                    _ => { },
                    e => e.LogError()));
        }
    }
}