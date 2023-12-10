using Generator;
using UnityEditor;

namespace WorldBuilder.Client.Editor
{
    public static class GeneratorUtil
    {
        [MenuItem("Tools/Generate")]
        public static void GenerateForEachType()
        {
            var generators = new ICodeGenerator[]
            {
                new SceneNameGenerator(),
                new PrefabGenerator(),
                new BuilderCodeGenerator(),
                new HolderGenerator(),
                new CommandGenerator()
            };
            foreach (var g in generators) g.Generate();
        }
    }
}