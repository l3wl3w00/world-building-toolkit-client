using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace WorldBuilder.Client.Editor
{
    public static class SceneNameGenerator
    {
        [MenuItem("Tools/Generate Scene Names")]
        public static void GenerateSceneNames()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public static class SceneNames");
            sb.AppendLine("{");

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                string scenePath = EditorBuildSettings.scenes[i].path;
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                sb.AppendLine($"    public const string {sceneName} = \"{sceneName}\";");
            }

            sb.AppendLine("}");

            string filePath = "Assets/Scripts/Generated/SceneNames.cs";
            File.WriteAllText(filePath, sb.ToString());
            AssetDatabase.Refresh();
        }
    }
}