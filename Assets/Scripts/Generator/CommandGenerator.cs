using System;
using System.Collections.Generic;
using Common.Model;
using Common.Triggers;
using Common.Utils;

namespace Generator
{
    public class CommandGenerator : ICodeGenerator
    {
        private CodeBuilder _code = new CodeBuilder();
        private readonly List<string> _path = new() { "Scripts", "Common", "Model", "Command"};

        public void Generate()
        {
            GenerateUtils.GetModelTypes().ForEach(Generate);
        }

        private void Generate(Type model)
        {
            _code = new CodeBuilder();
                        
            _code.Using("Common.ButtonBase");
            _code.Using("Common.Model.Builder");
            _code.Using("Common.Triggers");
            _code.Using("Common.Utils");
            _code.Using("Zenject");

            _code.Namespace("Common.Model.Command", _ => GenerateClasses(model));
            GenerateUtils.WriteCodeToCSharpFile(_path, $"{model.GetNameAsString()}Commands", _code.ToString());
        }

        private void GenerateClasses(Type model)
        {
            if (model == typeof(Planet)) return;
            GenerateStartCreatingCommand(model);
            GenerateCreateCommand(model);
            GenerateRemoveCommand(model);
        }

        private void GenerateStartCreatingCommand(Type model)
        {
            var modelName = model.GetNameAsString();
            _code.PublicClass($"StartCreating{modelName}Command", new[] { typeof(ActionListener) }, _ =>
            {
                _code
                    .AppendLineTabbed($"[Inject] private {modelName}BuilderHolder _builderHolder = null!; // Asserted in Start")
                    .AppendLineTabbed("protected void Start()")
                    .Block(_ =>
                    {
                        _code.AppendLineTabbed("NullChecker.AssertNoneIsNullInType(GetType(), _builderHolder);");
                    })
                    .AppendLineTabbed("public override void OnTriggered(NoActionParam param)")
                    .Block(_ =>
                    {
                        _code.AppendLineTabbed("_builderHolder.StartBuildingModel();");
                    });
            });
        }

        private void GenerateCreateCommand(Type model)
        {
            var modelName = model.GetNameAsString();
            _code.PublicClass($"Create{modelName}Command", new[] { typeof(ActionListener) }, _ =>
            {
                _code
                    .AppendLineTabbed($"[Inject] private ModelCollection<{modelName}> _collection = null!; // Asserted in Start")
                    .AppendLineTabbed($"[Inject] private {modelName}BuilderHolder _builderHolder = null!; // Asserted in Start")
                    .AppendLineTabbed("protected void Start()") 
                    .Block(_ =>
                    {
                        _code.AppendLineTabbed("NullChecker.AssertNoneIsNullInType(GetType(),_collection, _builderHolder);");
                    })
                    .AppendLineTabbed("public override void OnTriggered(NoActionParam param)")
                    .Block(_ =>
                    {
                        _code.AppendLineTabbed("_collection.Add(_builderHolder.BuildAndReset());");
                    });
            });
        }

        private void GenerateRemoveCommand(Type model)
        {
            // throw new NotImplementedException();
        }
    }
}