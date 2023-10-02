using UI.Common.Button;

namespace Game.Hud.Button.Create
{
    public class ContinentCreateCancelButton : HudButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            PlanetControl.DestroyActiveContinent();
            HudController.ToPreviousScreen();
            PlanetControl.ContinentInCreation = null;
        }
    }
}