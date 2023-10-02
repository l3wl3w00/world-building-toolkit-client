using System.IO;
using UnityEditor;
namespace WorldBuilder.Client.Editor
{
    [InitializeOnLoad]
    public class NullableEnabler
    {
        static NullableEnabler()
        {
            var watcher = new FileSystemWatcher
            {
                Path = "Assets/",
                Filter = "*.cs"
            };

            watcher.Created += OnCreated;
            watcher.EnableRaisingEvents = true;
        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            // WARNING: This is a very naive implementation. You should add more checks here.
            string content = File.ReadAllText(e.FullPath);
            if (!content.StartsWith("#nullable enable"))
            {
                content = "#nullable enable\n" + content;
                File.WriteAllText(e.FullPath, content);
            }
        }
    }
}