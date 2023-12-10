using Common.ButtonBase;
using Common.Generated;
using Common.Triggers;
using Common.Triggers.GameController;
using UI.Common.Button.SceneChange;

namespace UI.IntroScreen.Command
{
    public class ToIntroSceneButton : ButtonActionTrigger<ToIntroSceneCommand>
    {
        
    }

    public class ToIntroSceneCommand : SceneChangeCommand
    {
        protected override Scene Scene => Scene.UnauthorizedScreen;
    }
}