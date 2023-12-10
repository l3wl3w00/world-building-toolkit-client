using System;
using System.Collections.Generic;
using Common.Utils;

namespace Generator
{
    public class HolderGenerator : ICodeGenerator
    {
        private readonly List<string> _path = new() { "Scripts", "Game", "Common", "Holder"};
        public void Generate()
        {
            GenerateUtils.GetModelTypes().ForEach(GenerateForType);
        }

        private void GenerateForType(Type modelType)
        {
            var typeNamespace = _path.ToNamespace(1);
            var holderClassName = modelType.GetRawName() + "ModelHolder";
            var modelHolderCode = new CodeBuilder();
            modelHolderCode.AppendLineTabbed($"using {modelType.Namespace};");
            modelHolderCode.Namespace(typeNamespace, b => b
                    .AppendTabbed($"\tpublic class {holderClassName} : ModelHolder<{modelType.GetRawName()}> ")
                    .AppendLineTabbed("{ }"));
            GenerateUtils.WriteCodeToCSharpFile(_path, holderClassName, modelHolderCode.ToString());
            
            var idHolderClassName = modelType.GetRawName() + "ModelIdHolder";
            var idHolderCode = new CodeBuilder();
            idHolderCode.AppendLineTabbed($"using {modelType.Namespace};");
            idHolderCode.Namespace(typeNamespace, b => b
                    .AppendTabbed($"\tpublic class {idHolderClassName} : ModelIdHolder<{modelType.GetRawName()}> ")
                    .AppendLineTabbed("{ }"));
            GenerateUtils.WriteCodeToCSharpFile(_path, idHolderClassName, idHolderCode.ToString());
        }


    }
}