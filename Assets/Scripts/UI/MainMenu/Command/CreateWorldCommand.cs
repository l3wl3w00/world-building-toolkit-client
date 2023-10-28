#nullable enable
using Common;
using Common.ButtonBase;
using Common.Model;
using Common.Triggers;
using Common.Utils;
using Generated;

namespace UI.MainMenu.Command
{
    public class CreateWorldCommand : ActionListener
    {
        public override void OnTriggered(NoActionParam param)
        {
            Scenes.PlanetEditingScene.Load(SceneParamKeys.WorldInitializeParams, Option<PlanetWithRelatedEntities>.None);
        }
    }
}