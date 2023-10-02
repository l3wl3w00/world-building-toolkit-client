#nullable enable
using System.Collections.Generic;
using System.Linq;
using Game.Client;
using Game.Client.Dto;
using Game.Client.Response;
using Game.Constants;
using Game.Geometry.Sphere;
using UI.Common.Button;
using UnityEngine;

namespace Game.Hud.Button.Create
{
    public class ContinentCreateOkButton : HudButtonControl<NoButtonParams>
    {
        private WorldBuildingApiClient? _client;

        protected override void OnStart()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
        }

        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            if (PlanetControl.ContinentInCreation == null)
            {
                Debug.LogError("No continent is in creation");
                return;
            }

            var continent = PlanetControl.ContinentInCreation!;
            continent.ConnectLineEnds();
            continent.UpdateMesh();
            var controlPoints = continent.ControlPoints;

            var dto = ToCreateContinentDto(controlPoints);

            StartCoroutine(_client!.AddContinent(
                PlanetControl.Planet.Id,
                dto,
                _ => HudController.ToDefaultPanel(),
                ActionOnError));
        }

        private void ActionOnError(ErrorResponse e)
        {
            Debug.LogError(e.title);
        }

        private CreateContinentDto ToCreateContinentDto(List<SphereSurfaceCoordinate> coordinates)
        {
            return new CreateContinentDto
            {
                name = "new continent",
                description = "new continent description",
                bounds = coordinates
                    .Select(b => new PlanetCoordinateDto(
                        b.Height,
                        b.Polar.ToRadians(),
                        b.Azimuthal.ToRadians()))
                    .ToList()
            };
        }
    }
}