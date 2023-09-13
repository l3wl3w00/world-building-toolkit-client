using WorldBuilder.Client.UI.Common.Button;

namespace WorldBuilder.Client.UI.IntroScreen.Button
{
    public class QuitButtonControl : ButtonControl
    {
        public override void OnClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
