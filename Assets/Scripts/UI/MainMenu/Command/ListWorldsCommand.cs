#nullable enable
using Generated;
using UI.Common.Button.SceneChange;

namespace UI.MainMenu.Command
{
    public class ListWorldsCommand : SceneChangeCommand
    { 
        protected override Scenes Scene => Scenes.WorldListScreen;
    }
}