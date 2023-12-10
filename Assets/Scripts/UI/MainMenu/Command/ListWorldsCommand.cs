#nullable enable
using Common.Generated;
using UI.Common.Button.SceneChange;

namespace UI.MainMenu.Command
{
    public class ListWorldsCommand : SceneChangeCommand
    { 
        protected override Scene Scene => Scene.WorldListScreen;
    }
}