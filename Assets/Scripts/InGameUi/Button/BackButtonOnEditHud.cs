#nullable enable
using Common.ButtonBase;
using Common.Generated;
using Common.SceneChange;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using GameController;

namespace InGameUi.Button
{
    public class BackButtonOnEditHud : ButtonActionTrigger<ToWorldListScreenCommand> {}
    public class ToWorldListScreenCommand : ActionListenerMono<NoActionParam>
    {
        public override void OnTriggered(NoActionParam param)
        {
            SceneChangeParameters.FindInScene().DoIfNotNull(i => i.Destroy());
            Scene.WorldListScreen.Load();
        }
    }
}