using System.IO;
using System.Linq;
using System.Text;
using Game.Linq;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder.Client.Editor
{
    public static class SceneNameGenerator
    {
        [MenuItem("Tools/Generate")]
        public static void Generate()
        {
            GenerateSceneNames();
            GeneratePrefabNames();
        }

        private static void GeneratePrefabNames()
        {
            // Initialize StringBuilder for code generation
            var codeBuilder = new StringBuilder();

            // Create the using directives
            codeBuilder.AppendLine("using UnityEngine;");

            // Create the namespace
            codeBuilder.AppendLine("namespace WorldBuilder.Client.Generated");
            codeBuilder.AppendLine("{");
            var className = "Prefab";
            // Create the class signature
            codeBuilder.AppendLine($"\tpublic struct {className}");
            codeBuilder.AppendLine("\t{");
            codeBuilder.AppendLine("\t\tpublic string Name { get; }");
            codeBuilder.AppendLine($"\t\tprivate {className}(string name)");
            codeBuilder.AppendLine("\t\t{");
            codeBuilder.AppendLine("\t\t\tName = name;");
            codeBuilder.AppendLine("\t\t}");

            // Fetch all prefabs from the folder "Assets/Prefabs"
            var prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Resources/Prefabs" });

            foreach (var guid in prefabGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var pathRelativeToResources = string.Join("/", assetPath.Split('/').Skip(2)).Split(".")[0];
                var prefabName = Path.GetFileNameWithoutExtension(assetPath);
                var prefabNormalizedNameBuilder = new StringBuilder();
                prefabName.Where(c => c != ' ').ForEach(c => prefabNormalizedNameBuilder.Append(c));
                var prefabNormalizedName = prefabNormalizedNameBuilder.ToString();
                // Create a static string variable for each prefab
                codeBuilder.AppendLine(
                    $"\t\tpublic static {className} {prefabNormalizedName} => new {className}(\"{pathRelativeToResources}\");");
            }

            // Close the class and namespace
            codeBuilder.AppendLine("\t}");
            codeBuilder.AppendLine("}");

            // Write code to disk
            File.WriteAllText(Application.dataPath + "/Scripts/Generated/PrefabName.cs", codeBuilder.ToString());

            // Refresh the AssetDatabase to recognize the new file
            AssetDatabase.Refresh();
        }

        public static void GenerateSceneNames()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using UnityEngine;");

            // Create the namespace
            sb.AppendLine("namespace WorldBuilder.Client.Generated");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic class Scenes");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic string Name { get; }");
            sb.AppendLine("\t\tprivate Scenes(string name)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tName = name;");
            sb.AppendLine("\t\t}");
            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var scenePath = EditorBuildSettings.scenes[i].path;
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                sb.AppendLine($"\t\tpublic static Scenes {sceneName} => new Scenes(\"{sceneName}\");");
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            var filePath = "Assets/Scripts/Generated/SceneNames.cs";
            File.WriteAllText(filePath, sb.ToString());
            AssetDatabase.Refresh();
        }
    }
}