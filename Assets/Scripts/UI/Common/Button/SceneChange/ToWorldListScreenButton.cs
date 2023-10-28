#nullable enable
using Generated;

namespace UI.Common.Button.SceneChange
{
    public class ToWorldListScreenButton : SceneChangeButton<ToWorldListScreenCommand> { }

    public class ToWorldListScreenCommand : SceneChangeCommand
    {
        protected override Scenes Scene => Scenes.WorldListScreen;

    }
}