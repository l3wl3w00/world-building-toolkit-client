#nullable enable
using System.Linq;
using Game.Client;
using Game.Client.Dto;
using Game.Constants;
using Game.Util;
using TMPro;
using UI.Common.Button;
using UnityEngine;

namespace Game.Hud.Button.Default
{
    public class UpdateWorldButton : HudButtonControl<NoButtonParams>
    {
        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;

        protected override void OnStart()
        {
            _client = new WorldBuildingApiClient(
                    PlayerPrefs.GetString(AuthConstants.GoogleTokenKey))
                .ToOption();
        }

        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            var texts = FindObjectsOfType<TextMeshProUGUI>();
            var worldName = texts.Single(t => t.name == "NameInput").text.Replace("\u200b", "");
            var description = texts.Single(t => t.name == "DescriptionInput").text.Replace("\u200b", "");

            var request = _client
                .ExpectNotNull($"client was uninitialized in {nameof(UpdateWorldButton)}")
                .UpdateWorld(
                    PlanetControl.Planet.Id,
                    new CreateWorldDto
                    {
                        Name = worldName,
                        Description = description
                    },
                    _ => { },
                    e => e.LogError());
            StartCoroutine(request);
        }
    }
}