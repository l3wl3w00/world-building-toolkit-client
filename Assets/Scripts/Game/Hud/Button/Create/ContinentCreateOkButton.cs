#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Client;
using Game.Client.Dto;
using Game.Client.Response;
using Game.Constants;
using Game.Geometry.Sphere;
using Game.Util;
using UI.Common.Button;
using UnityEngine;

namespace Game.Hud.Button.Create
{
    public class ContinentCreateOkButton : HudButtonControl<NoButtonParams>
    {
        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;

        protected override void OnStart()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey)).ToOption();
        }

        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            PlanetControl.ContinentInCreation
                .DoIfNull(() => Debug.LogError("No continent is in creation"))
                .DoIfNotNull(continent =>
                {
                    continent.ConnectLineEnds();
                    continent.UpdateMesh();
                    var controlPoints = continent.ControlPoints;

                    var dto = ToCreateContinentDto(controlPoints);

                    var request = _client
                        .ExpectNotNull(nameof(_client), (Action<NoButtonParams>) OnClickedTypesafe)
                        .AddContinent(PlanetControl.Planet.Id, dto, c =>
                        {
                            continent.Id = c.Id;
                            HudController.ToDefaultPanel();
                        }, ActionOnError);
                    StartCoroutine(request);
                });
        }

        private void ActionOnError(ErrorResponse e)
        {
           e.LogError();
        }

        private CreateContinentDto ToCreateContinentDto(List<SphereSurfaceCoordinate> coordinates)
        {
            return new CreateContinentDto
            {
                Name = "new continent",
                Description = "new continent description",
                Bounds = coordinates
                    .Select(b => new PlanetCoordinateDto
                    {
                        Radius = b.Height,
                        Polar =  b.Polar.ToRadians(),
                        Azimuthal = b.Azimuthal.ToRadians()
                    })
                    .ToList()
            };
        }
    }
}