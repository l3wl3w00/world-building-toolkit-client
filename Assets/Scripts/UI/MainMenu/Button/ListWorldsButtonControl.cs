#nullable enable
using Common;
using Generated;
using UI.Common.Button;

namespace UI.MainMenu.Button
{
    public class ListWorldsButtonControl : ButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams param)
        {
            Scenes.WorldListScreen.Load();
        }
    }
}