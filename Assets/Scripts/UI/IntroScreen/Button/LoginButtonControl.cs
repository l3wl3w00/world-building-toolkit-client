using UnityEngine.SceneManagement;
using WorldBuilder.Client.UI.Common.Button;

namespace WorldBuilder.Client.UI.IntroScreen.Button
{
    public class LoginButtonControl : ButtonControl
    {
        public override void OnClicked()
        {
            SceneManager.LoadScene(SceneNames.LoginScreen);
        }
    }
}
