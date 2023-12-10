#nullable enable
using System;
using Common.ButtonBase;
using Common.Generated;
using Common.Triggers;
using UI.Common.Button.SceneChange;

namespace UI.IntroScreen.Command
{
    public class ToRegisterSceneCommand : SceneChangeCommand
    {
        protected override Scene Scene => Scene.RegisterScene;
    }
}