using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Generator
{
    public class BuilderCodeGenerator : ICodeGenerator
    {
        private readonly List<string> _path = new() { "Scripts", "Common", "Model", "Builder"};
        public void Generate()
        {
            foreach (var type in GenerateUtils.GetModelTypes())
            {
                GenerateForType(type);
            };
        }

        private void GenerateForType(Type type)
        {
            var builderName = type.Name + "Builder";
            var builderNamespaceName = _path.ToNamespace(1);
            
            var code = new CodeBuilder();
            code.Using(type.Namespace!)
                .Using(builderNamespaceName)
                .Using("Common");
            type.GetAutoProperties().ForEach(p => code.AppendLineTabbed($"using {p.PropertyType.Namespace};"));
            code.Namespace(builderNamespaceName, _ =>
            {
                code.PublicSealedPartialClass(builderName, new[] { $"ModelBuilder<{type.Name}>" }, _ =>
                {                    
                    GenerateConstructor(code, builderName);
                    GenerateProperties(type, code, builderName);
                    GenerateBuildMethod(type, code);
                });
                code.PublicPartialClass($"{builderName}Holder", new[] { $"BuilderHolder<{type.GetNameAsString()}, {builderName}>" });
            });
            code.Namespace("Common.Model", _ =>
                code.Class(
                    keywords:  new[] { Keyword.Public, Keyword.Partial, Keyword.KeywordOfType(type)},
                    className: type.GetNameAsString(),
                    inherits:  Enumerable.Empty<Type>(),
                    build:     _ => code.AppendLineTabbed($"public static {builderName} Builder() => new {builderName}();"))
            );
            
            GenerateUtils.WriteCodeToCSharpFile(_path, builderName, code.ToString());
        }

        private void GenerateConstructor(CodeBuilder code, string builderName)
        {
            code.AppendLineTabbed($"public {builderName}()")
                .Block(_ => code.AppendLineTabbed("OnConstruct();"));
        }

        private static void GenerateProperties(Type type, CodeBuilder code, string builderName)
        {
            type.GetAutoProperties().ForEach(p =>
            {
                
                var propertyTypeName = p.PropertyType.GetNameAsString();
                code.AppendTabbed($"public Option<{propertyTypeName}> {p.Name}Opt" + " { get; private set; }");

                if (p.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                {
                    code.AppendLine($" = (new List<{p.PropertyType.GetArgumentListAsString()}>() as {p.PropertyType.GetNameAsString()}).ToOption();");
                }
                else
                {
                    code.AppendLine();
                }

                code.AppendLineTabbed($"public {propertyTypeName} {p.Name} => {p.Name}Opt" +
                                  $".ExpectNotNull(\"property '{p.Name}' was null when querying its value\");");
                code.AppendLineTabbed($"public {builderName} With{p.Name}({propertyTypeName} value)")
                    .Block(_ =>
                    {
                        code.AppendLineTabbed($"{p.Name}Opt = value.ToOption();")
                            .AppendLineTabbed("return this;");
                    });
            });
        }
        
        private static void GenerateBuildMethod(Type type, CodeBuilder code)
        {
            var typeName = type.GetNameAsString();
            code
            .AppendLineTabbed($"public override {typeName} Build()")
            .Block(_ =>
            {
                code.AppendLineTabbed("BeforeBuild();");
                code.AppendLineTabbed("return new");
                code.AppendLineTabbed("(");
                code.AddTab();
                foreach (var p in type.GetAutoProperties())
                {
                    code.AppendTabbed($"{p.Name}: {p.Name}Opt.ExpectNotNull(\"property '{p.Name}' was not provided when building object of {typeName}\")");
                    if (p != type.GetAutoProperties().Last()) code.AppendLine(",");
                    else code.AppendLine();
                }
                code.RemoveTab();
                code.AppendLineTabbed(");");
            });
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