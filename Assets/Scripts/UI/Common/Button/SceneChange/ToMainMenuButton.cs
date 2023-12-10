#nullable enable
using Common.Generated;

namespace UI.Common.Button.SceneChange
{
    public class ToMainMenuButton : SceneChangeButton<ToMainMenuCommand> { }

    public class ToMainMenuCommand : SceneChangeCommand
    {
        protected override Scene Scene => Scene.MainMenuScreen;
        
    }
}