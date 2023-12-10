using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Generator
{
    public class SceneGenerator : ICodeGenerator
    {
        private readonly List<string> _path = new() { "Scripts", nameof(Common), nameof(Common.Generated)};
        private const string ClassName = "Scene";
        public void Generate()
        {
            var cb = new CodeBuilder();
            cb.Using("UnityEngine");
 
            cb.Namespace("Common.Generated", _ =>
            {
                cb.PublicPartialStruct(ClassName, Enumerable.Empty<string>(), _ =>
                {
                    foreach (var s in EditorBuildSettings.scenes)
                    {
                        var scenePath = s.path;
                        var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                        cb.AppendLineTabbed($"public static {ClassName} {sceneName} => new {ClassName}(\"{sceneName}\");");
                    }
                });
            });

            GenerateUtils.WriteCodeToCSharpFile(_path, "Scenes", cb.ToString());
            AssetDatabase.Refresh();
        }
    }
}