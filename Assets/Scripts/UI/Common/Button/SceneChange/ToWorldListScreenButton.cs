#nullable enable
using Common.Generated;

namespace UI.Common.Button.SceneChange
{
    public class ToWorldListScreenButton : SceneChangeButton<ToWorldListScreenCommand> { }

    public class ToWorldListScreenCommand : SceneChangeCommand
    {
        protected override Scene Scene => Scene.WorldListScreen;

    }
}