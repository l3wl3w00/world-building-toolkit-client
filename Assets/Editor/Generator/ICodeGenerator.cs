using Common.Utils;
using UnityEditor;

namespace WorldBuilder.Client.Editor.Generator
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
            };
            generators.ForEach(g => g.Generate());
        }
    }
    public interface ICodeGenerator
    {
        void Generate();
    }
}