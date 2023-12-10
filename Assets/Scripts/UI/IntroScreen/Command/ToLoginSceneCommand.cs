#nullable enable
using Common.Generated;
using UI.Common.Button.SceneChange;

namespace UI.IntroScreen.Command
{
    public class ToLoginSceneCommand : SceneChangeCommand
    {
        protected override Scene Scene => 
            Scene.LoginScreen;
    }
}