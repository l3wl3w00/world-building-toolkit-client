#nullable enable
using Game.Client.Dto;
using Game.Continent;
using Game.Util;
using UI.Common.Button;

namespace Game.Hud.Button.Default
{
    public class ContinentCreateStartButton : HudButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            PlanetControl.ContinentInCreation = ContinentHandler.CreateGameObject(PlanetControl).ToOption();
            HudController.CurrentHudScreen = HudScreen.ContinentCreate;
        }
    }
}