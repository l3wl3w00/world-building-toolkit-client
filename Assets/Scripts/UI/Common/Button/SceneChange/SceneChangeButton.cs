#nullable enable
using Common;
using Common.ButtonBase;
using Common.SceneChange;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using Generated;
using UnityEngine;

namespace UI.Common.Button.SceneChange
{
    public abstract class SceneChangeButton<TCommand> : ButtonActionTrigger<TCommand>
        where TCommand : SceneChangeCommand
    {}

    public abstract class SceneChangeCommand : ActionListenerMono<NoActionParam>
    {
        public override void OnTriggered(NoActionParam param)
        {
            SceneChangeParameters.Instance.DoIfNotNull(i => i.Destroy());
            var parameters = new SceneParametersBuilder();
            ConfigureParameters(parameters);
            parameters.Save();
            Scene.Load();
        }
        
        protected virtual void ConfigureParameters(SceneParametersBuilder parameterBuilder)
        {
        }
        protected abstract Scenes Scene { get; }
    }
}