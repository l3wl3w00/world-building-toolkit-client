#nullable enable
using System;
using System.Collections.Generic;
using Common;
using Game.Client.Dto;
using Game.Hud;
using Game.SceneChange;
using Generated;
using UI.Common.Button;

namespace UI.MainMenu.Button
{
    public class CreateWorldButton : ButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams param)
        {
            var worldDetailed = new WorldDetailedDto
            {
                id = Guid.Empty.ToString(),
                continents = new List<ContinentDto>(),
                description = "",
                name = ""
            };
            new SceneParametersBuilder()
                .Add(SceneParamKey.WorldDetailed, worldDetailed)
                .Add(SceneParamKey.InitialScreen, HudScreen.PlanetCreate)
                .Save();
            Scenes.PlanetEditingScene.Load();
        }
    }
}