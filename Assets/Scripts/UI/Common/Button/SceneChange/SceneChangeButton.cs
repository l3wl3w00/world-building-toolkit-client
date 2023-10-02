#nullable enable
using Common;
using Game.SceneChange;
using Generated;

namespace UI.Common.Button.SceneChange
{
    public abstract class SceneChangeButton : ButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams param)
        {
            if (DropParameters) ISceneChangeParameters.Instance.Destroy();
            var parameters = new SceneParametersBuilder();
            ConfigureParameters(parameters);
            parameters.Save();
            Scene.Load();
        }

        protected virtual void ConfigureParameters(SceneParametersBuilder parameterBuilder)
        {
        }

        #region Properties

        protected abstract Scenes Scene { get; }
        protected virtual bool DropParameters { get; } = true;

        #endregion
    }
}