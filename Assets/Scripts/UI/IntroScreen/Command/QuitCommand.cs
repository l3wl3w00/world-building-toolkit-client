#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using UnityEditor;

namespace UI.IntroScreen.Button
{
    public class QuitCommand : ActionListener
    {
        public override void OnTriggered(NoActionParam param)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}