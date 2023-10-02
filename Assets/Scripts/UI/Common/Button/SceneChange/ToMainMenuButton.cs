#nullable enable
using Generated;

namespace UI.Common.Button.SceneChange
{
    public class ToMainMenuButton : SceneChangeButton
    {
        #region Properties

        protected override Scenes Scene => Scenes.MainMenuScreen;

        #endregion
    }
}