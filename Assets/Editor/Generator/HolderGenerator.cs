using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common.Utils;
using UnityEngine;

namespace WorldBuilder.Client.Editor.Generator
{
    public class HolderGenerator : ICodeGenerator
    {
        private readonly List<string> _path = new() { "Scripts", "Game", "Common", "Holder"};
        public void Generate()
        {
            ModelGenerateUtils.GetModelTypes().ForEach(GenerateForType);
        }

        private void GenerateForType(Type modelType)
        {
            var typeNamespace = _path.ToNamespace(1);
            var holderClassName = modelType.GetRawName() + "ModelHolder";
            var modelHolderCode = new StringBuilder();
            modelHolderCode.AppendLine($"using {modelType.Namespace};");
            modelHolderCode.InsideNameSpace(typeNamespace, b => b
                    .Append($"\tpublic class {holderClassName} : ModelHolder<{modelType.GetRawName()}> ")
                    .AppendLine("{ }"));
            ModelGenerateUtils.WriteCodeToCSharpFile(_path, holderClassName, modelHolderCode.ToString());
            
            var idHolderClassName = modelType.GetRawName() + "ModelIdHolder";
            var idHolderCode = new StringBuilder();
            idHolderCode.AppendLine($"using {modelType.Namespace};");
            idHolderCode.InsideNameSpace(typeNamespace, b => b
                    .Append($"\tpublic class {idHolderClassName} : ModelIdHolder<{modelType.GetRawName()}> ")
                    .AppendLine("{ }"));
            ModelGenerateUtils.WriteCodeToCSharpFile(_path, idHolderClassName, idHolderCode.ToString());
        }


    }
}