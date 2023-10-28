using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Common.Model.Abstractions;
using Common.Utils;
using PlasticGui.WorkspaceWindow.Merge;
using UnityEngine;

namespace WorldBuilder.Client.Editor.Generator
{
    public static class ModelGenerateUtils
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
            var argumentList = string.Join(',', type.GenericTypeArguments.Select(GetNameAsString).AsEnumerable());
            var rawName = type.GetRawName();
            return $"{rawName}<{argumentList}>";
        }

        public static string GetRawName(this Type type)
        {
            if (!type.IsGenericType) return type.Name;
            return type.Name.Split('`').First();
        }
        
        public static StringBuilder InsideNameSpace(this StringBuilder builder, string @namespace, Action<StringBuilder> build)
        {
            builder
                .AppendLine($"namespace {@namespace}")
                .AppendLine("{");
            build(builder);
            return builder.AppendLine("}");
        }
        
        public static string ToNamespace(this List<string> path, int skipFirstNElements) => string.Join(".", path.GetRange(skipFirstNElements, path.Count - 1));
    }
    public class BuilderCodeGenerator : ICodeGenerator
    {
        private readonly List<string> _path = new() { "Scripts", "Common", "Model", "Builder"};
        public void Generate()
        {
            ModelGenerateUtils.GetModelTypes().ForEach(GenerateForType);
        }

        private void GenerateForType(Type type)
        {
            var builderName = type.Name + "Builder";
            var builderNamespaceName = _path.ToNamespace(1);
            var code = new StringBuilder();
            code.AppendLine($"using {type.Namespace};")
                .AppendLine($"using {builderNamespaceName};");
            type.GetAutoProperties().ForEach(p => code.AppendLine($"using {p.PropertyType.Namespace};"));
            code.AppendLine($"namespace {builderNamespaceName}")
                .AppendLine("{")
                .AppendLine($"public class {builderName} : IModelBuilder<{type.Name}>")
                .AppendLine("{");
            type.GetAutoProperties().ForEach(p =>
            {
                var propertyTypeName = p.PropertyType.GetNameAsString();
                code.AppendLine($"\tpublic {propertyTypeName} {p.Name}" + "{ get; private set; }")
                    .AppendLine($"\tpublic {builderName} With{p.Name}({propertyTypeName} value)")
                    .AppendLine("\t{")
                    .AppendLine($"\t\t{p.Name} = value;")
                    .AppendLine("\t\treturn this;")
                    .AppendLine("\t}");
            });
            var typeName = type.GetNameAsString();
            code.AppendLine($"\tpublic {typeName} Build()")
                .AppendLine("\t{")
                .AppendLine("\t\treturn new")
                .AppendLine("\t\t(");
            foreach (var p in type.GetAutoProperties())
            {
                code.Append($"\t\t\t{p.Name}: {p.Name}");
                if (p != type.GetAutoProperties().Last())
                {
                    code.AppendLine(",");
                }
                else
                {
                    code.AppendLine();
                }
            }
            code.AppendLine("\t\t);")
                .AppendLine("\t}")
                .AppendLine("}")
                .AppendLine($"public class {builderName}Holder : BuilderHolder<{typeName}, {builderName}> " + "{ }")
                .AppendLine("}")
                .AppendLine($"namespace Common.Model")
                .AppendLine("{")
                .AppendLine($"\tpublic partial {GetTypeKeyword(type)} {type.GetNameAsString()}")
                .AppendLine("\t{")
                .AppendLine($"\t\tpublic static {builderName} Builder() => new {builderName}();")
                .AppendLine("\t}")
                .AppendLine("}");

            ModelGenerateUtils.WriteCodeToCSharpFile(_path, builderName, code.ToString());
        }

        public string GetTypeKeyword(Type type)
        {
            var isRecord = type.GetMethods().Any(m => m.Name == "<Clone>$");
            if (isRecord) return "record";
            return type switch
            {
                { IsEnum : true } => "enum",
                { IsValueType : true } => "struct",
                { IsClass : true } => "class",
                _ => "",
            };
        }
    }
    
    public static class ReflectionUtils
    {
        public static List<PropertyInfo> GetAutoProperties(this Type targetType)
        {
            return targetType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => p.GetGetMethod(true).IsDefined(typeof(CompilerGeneratedAttribute), false))
                .Where(p => p.Name != "EqualityContract")
                .ToList();
        }
    }
}