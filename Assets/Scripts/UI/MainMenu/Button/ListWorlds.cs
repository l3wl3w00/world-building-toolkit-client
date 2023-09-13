using UnityEngine.SceneManagement;
using WorldBuilder.Client.UI.Common.Button;

namespace WorldBuilder.Client.UI.MainMenu.Button
{
    public class ListWorlds : ButtonControl
    {
        public override void OnClicked()
        {
            SceneManager.LoadScene(SceneNames.WorldListScreen);
        }
    }
}