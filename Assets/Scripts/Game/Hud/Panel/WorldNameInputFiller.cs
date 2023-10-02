#nullable enable
using Game.Util;

namespace Game.Hud.Panel
{
    public class WorldNameInputFiller : TextInputFiller
    {
        protected override Option<string> GetValue()
        {
            return PlanetControl.Planet.Name.ToOption();
        }
    }
}