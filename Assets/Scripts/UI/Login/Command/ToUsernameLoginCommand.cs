#nullable enable
using System;
using Common.ButtonBase;
using Common.Generated;
using Common.Triggers;
using UI.Common.Button.SceneChange;

namespace UI.Login.Command
{
    public class ToUsernameLoginCommand: SceneChangeCommand
    {
        protected override Scene Scene => Scene.UserLogin;
    }
}