#nullable enable
using Common.ButtonBase;
using Common.SceneChange;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using GameController;
using Generated;

namespace InGameUi.Button
{
    public class BackButtonOnEditHud : ButtonActionTrigger<ToWorldListScreenCommand> {}
    public class ToWorldListScreenCommand : ActionListenerMono<NoActionParam>
    {
        public override void OnTriggered(NoActionParam param)
        {
            SceneChangeParameters.Instance.DoIfNotNull(i => i.Destroy());
            Scenes.WorldListScreen.Load();
        }
    }
}