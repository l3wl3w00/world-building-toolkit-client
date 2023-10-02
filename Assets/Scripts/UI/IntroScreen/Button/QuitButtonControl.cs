using UI.Common.Button;
using UnityEditor;

namespace UI.IntroScreen.Button
{
    public class QuitButtonControl : ButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams param)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}