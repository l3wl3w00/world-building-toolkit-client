#nullable enable
using Generated;

namespace UI.Common.Button.SceneChange
{
    public class ToMainMenuButton : SceneChangeButton<ToMainMenuCommand> { }

    public class ToMainMenuCommand : SceneChangeCommand
    {
        protected override Scenes Scene => Scenes.MainMenuScreen;
        
    }
}