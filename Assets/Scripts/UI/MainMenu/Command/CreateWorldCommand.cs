#nullable enable
using Common;
using Common.ButtonBase;
using Common.Generated;
using Common.Model;
using Common.Triggers;
using Common.Utils;

namespace UI.MainMenu.Command
{
    public class CreateWorldCommand : ActionListener
    {
        public override void OnTriggered(NoActionParam param)
        {
            Scene.PlanetEditingScene.Load(SceneParamKeys.WorldInitializeParams, Option<PlanetWithRelatedEntities>.None);
        }
    }
}