using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Model.Abstractions;
using UnityEditor;
using UnityEngine;

namespace Generator
{
    public static class GeneratorUtil
    {
        [MenuItem("Tools/Generate")]
        public static void GenerateForEachType()
        {
            var generators = new ICodeGenerator[]
            {
                new SceneGenerator(),
                new PrefabGenerator(),
                new BuilderCodeGenerator(),
                new HolderGenerator(),
                new CommandGenerator()
            };
            foreach (var g in generators) g.Generate();
        }
    }
    
    public static class GenerateUtils
    {
        public static IEnumerable<Type> GetModelTypes() =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IModel).IsAssignableFrom(t))
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType);

        public static void WriteCodeToCSharpFile(IEnumerable<string> path, string fileName, string code)
        {
            var pathStr = string.Join("/", path);
            File.WriteAllText(Application.dataPath + $"/{pathStr}/{fileName}.cs",code);
        }
        public static string GetNameAsString(this Type type)
        {
            if (!type.IsGenericType) return type.Name;
            var argumentList = type.GetArgumentListAsString();
            var rawName = type.GetRawName();
            return $"{rawName}<{argumentList}>";
        }

        public static string GetArgumentListAsString(this Type type)
        {
            if (!type.IsGenericType) return string.Empty;
            return string.Join(',', type.GenericTypeArguments.Select(GetNameAsString).AsEnumerable());
        }

        public static string GetRawName(this Type type)
        {
            if (!type.IsGenericType) return type.Name;
            return type.Name.Split('`').First();
        }
        
        
        public static string ToNamespace(this List<string> path, int skipFirstNElements) => string.Join(".", path.GetRange(skipFirstNElements, path.Count - 1));
    }
    public interface ICodeGenerator
    {
        void Generate();
    }
}