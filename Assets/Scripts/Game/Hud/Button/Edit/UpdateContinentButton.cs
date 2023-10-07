#nullable enable
using System;
using System.Linq;
using Game.Client;
using Game.Client.Dto;
using Game.Constants;
using Game.Continent;
using Game.Util;
using TMPro;
using UI.Common.Button;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hud.Button.Edit
{
    public class UpdateContinentButton : HudButtonControl<NoButtonParams>
    {
        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;

        protected override void OnStart()
        {
            base.OnStart();
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
        }

        protected override void OnClickedTypesafe(NoButtonParams param)
        {

            PlanetControl.SelectedContinent
                .DoIfNotNull(c => UpdateContinent(c))
                .DoIfNull(() => Debug.LogError("Selected continent was null"));
        }

        private void UpdateContinent(ContinentHandler selectedContinent)
        {
            var texts = FindObjectsOfType<TextMeshProUGUI>().ToOption().Value;
            var nameInput = texts.Single(t => t.name == "NameInput");
            var descriptionInput = texts.Single(t => t.name == "DescriptionInput");
            
            var invertToggle = FindObjectsOfType<Toggle>().Single(t => t.name == "Inverted");
            var continentName = nameInput.text.Replace("\u200b", "");
            var continentDescription = descriptionInput.text.Replace("\u200b", "");
            
            var inverted = invertToggle.isOn;

            var dto = new PatchContinentDto
            {
                Inverted = inverted,
                Name = continentName,
                Description = continentDescription
            };
            
            var patchCoroutine = _client
                .ExpectNotNull(nameof(_client), (Action<ContinentHandler>)UpdateContinent)
                .PatchContinent(
                    selectedContinent.Id,
                    dto,
                    d =>
                    {
                        nameInput.text = d.Name;
                        descriptionInput.text = d.Description;
                        invertToggle.isOn = d.Inverted;
                        OnContinentPatchSuccessful(d);
                    },
                    _ => { });
            StartCoroutine(patchCoroutine);
            
        }

        private void OnContinentPatchSuccessful(ContinentDto newContinentDto)
        {
            PlanetControl.SelectedContinent
                .DoIfNotNull(c =>
                {
                    c.ContinentName = newContinentDto.Name;
                    c.ContinentDescription = newContinentDto.Description;
                })
                .LogErrorIfNull("SelectedContinent was null when returning from patch continent coroutine");
            HudController.UpdateInputFields();
            Debug.Log("Successful update!");
        }
    }
}