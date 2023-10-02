using System.IO;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder.Client.Editor
{
    public static class NullableEnabler
    {
        [MenuItem("Tools/Add #nullable enable to Scripts")]
        public static void AddNullableToScripts()
        {
            var files = Directory.GetFiles("Assets/Scripts", "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                if (content.StartsWith("#nullable enable")) continue;
                
                content = "#nullable enable\n" + content; 
                File.WriteAllText(file, content);
            }

            Debug.Log("Added #nullable enable to scripts.");
        }
    }
}