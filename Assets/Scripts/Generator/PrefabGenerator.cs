using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Utils;
using UnityEditor;
using UnityEngine;

namespace Generator
{
    public class PrefabGenerator : ICodeGenerator
    {
        private readonly List<string> _path = new() { "Scripts", nameof(Common), nameof(Common.Generated)};
        public void Generate()
        {
            var codeBuilder = new CodeBuilder();

            codeBuilder.Using("UnityEngine");

            codeBuilder.Namespace("Common.Generated", _ =>
            {
                const string className = "Prefab";
                codeBuilder.PublicPartialStruct(className, Enumerable.Empty<string>(), _ =>
                {
                    var prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Resources/Prefabs" });

                    foreach (var guid in prefabGuids)
                    {
                        var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                        var pathRelativeToResources = string.Join("/", assetPath.Split('/').Skip(2)).Split(".")[0];
                        var prefabName = Path.GetFileNameWithoutExtension(assetPath);
                        var prefabNormalizedNameBuilder = new StringBuilder();
                        prefabName.Where(c => c != ' ').ForEach(c => prefabNormalizedNameBuilder.Append(c));
                        var prefabNormalizedName = prefabNormalizedNameBuilder.ToString();
                        codeBuilder.AppendLineTabbed(
                            $"public static {className} {prefabNormalizedName} => new {className}(\"{pathRelativeToResources}\");");
                    }
                });
            });
            
            GenerateUtils.WriteCodeToCSharpFile(_path, "Prefabs", codeBuilder.ToString());
            AssetDatabase.Refresh();
        }
    }
}