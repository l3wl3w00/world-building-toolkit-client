using Common;
using Common.Utils;
using Generated;

namespace WorldBuilder.Client.Editor
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine.SceneManagement;
    
    
    [InitializeOnLoad]
    public class DefaultSceneLoader
    {
        [MenuItem("Tools/Disable Default Scene")]
        static void DisableDefaultScene()
        {
            EditorPrefs.SetBool("defaultEnabled", false);
        }
        
        [MenuItem("Tools/Enable Default Scene")]
        static void EnableDefaultScene()
        {
            EditorPrefs.SetBool("defaultEnabled", true);
        }

        static DefaultSceneLoader()
        {
            if (EditorPrefs.GetBool("defaultEnabled"))
            {
                EditorApplication.playModeStateChanged += LoadDefaultScene;
            }
        }

        static void LoadDefaultScene(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorPrefs.SetString("prevScene", SceneManager.GetActiveScene().path);
            }

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Scenes.Initial.Load();
            }

            if (state == PlayModeStateChange.EnteredEditMode)
            {
                string prevScene = EditorPrefs.GetString("prevScene");
                if (!string.IsNullOrEmpty(prevScene))
                {
                    SceneManager.LoadScene(prevScene);
                }
            }
        }
    }
}