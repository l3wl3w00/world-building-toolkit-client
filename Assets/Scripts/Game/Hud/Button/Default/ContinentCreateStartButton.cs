using Game.Continent;
using UI.Common.Button;

namespace Game.Hud.Button.Default
{
    public class ContinentCreateStartButton : HudButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            PlanetControl.ContinentInCreation = ContinentHandler.CreateGameObject(PlanetControl);
            HudController.CurrentHudScreen = HudScreen.ContinentCreate;
        }
    }
}