#nullable enable
using System;
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
    public class CreateWorldButton : HudButtonControl<NoButtonParams>
    {
        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;

        protected override void OnStart()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey)).ToOption();
        }

        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            var texts = FindObjectsOfType<TextMeshProUGUI>().ToOption().Value;
            var worldName = texts.Single(t => t.name == "NameInput").text.Replace("\u200b", "");
            var description = texts.Single(t => t.name == "DescriptionInput").text.Replace("\u200b", "");

            
            StartCoroutine(
                _client
                    .ExpectNotNull(nameof(_client), (Action<NoButtonParams>)OnClickedTypesafe)
                    .CreateWorld(
                        new CreateWorldDto
                        {
                            Name = worldName,
                            Description = description
                        },
                        w =>
                        {
                            PlanetControl.Planet = new Model.Planet(w.Id, w.Name, w.Description);
                            HudController.CurrentHudScreen = HudScreen.PlanetEdit;
                            HudController.UpdateInputFields();
                        },
                        e => e.LogError()));
        }
    }
}