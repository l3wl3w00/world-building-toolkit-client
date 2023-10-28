using System.IO;
using System.Text;
using UnityEditor;

namespace WorldBuilder.Client.Editor.Generator
{
    public class SceneNameGenerator : ICodeGenerator
    {
        public void Generate()
        {
            var sb = new StringBuilder();
             sb.AppendLine("using UnityEngine;");
 
             // Create the namespace
             sb.AppendLine("namespace Generated");
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