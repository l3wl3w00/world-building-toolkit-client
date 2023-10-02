#nullable enable
using Game.Util;

namespace Game.Hud.Panel
{
    public class WorldDescriptionInputFiller : TextInputFiller
    {
        protected override Option<string> GetValue()
        {
            return PlanetControl.Planet.Description.ToOption();
        }
    }
}