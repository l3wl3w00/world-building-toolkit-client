#nullable enable
using Game.Util;

namespace Game.Hud.Panel
{
    public class ContinentDescriptionInputFiller : TextInputFiller
    {
        protected override Option<string> GetValue()
        {
            return PlanetControl.SelectedContinent.NoneOr(c => c.ContinentDescription);
        }
    }
}