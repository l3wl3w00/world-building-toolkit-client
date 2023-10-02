#nullable enable
using Game.Util;

namespace Game.Hud.Panel
{
    public class InitialInvertedInputValueFiller : ToggleInputFiller
    {
        protected override Option<bool> GetValue()
        {
            return PlanetControl.SelectedContinent.MapIfNotNull(c => c.Invert);
        }
    }
}