#nullable enable
using Common;
using Common.ButtonBase;
using Common.Generated;
using Common.Model;
using Common.Model.Abstractions;
using Common.Triggers.GameController;
using Common.Utils;

namespace UI.WorldList
{
    public class ToEditWorldScreenCommand : ActionListenerMono<SingleActionParam<IdOf<Planet>>>
    {
        public override void OnTriggered(SingleActionParam<IdOf<Planet>> param)
        {
            Scene.PlanetEditingLoadScene.Load(SceneParamKeys.WorldId, param.Value);
        }
    }
}