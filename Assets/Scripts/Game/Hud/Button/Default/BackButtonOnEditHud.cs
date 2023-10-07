#nullable enable
using Common;
using Game.SceneChange;
using Generated;
using UI.Common.Button;

namespace Game.Hud.Button.Default
{
    public class BackButtonOnEditHud : HudButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams buttonParams)
        {
            if (HudController.HasPreviousScreen())
            {
                HudController.ToPreviousScreen();
                return;
            }

            ISceneChangeParameters.Instance.Destroy();
            Scenes.WorldListScreen.Load();
        }
    }
}