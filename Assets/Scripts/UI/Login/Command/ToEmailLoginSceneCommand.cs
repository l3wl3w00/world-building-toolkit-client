#nullable enable
using System;
using Common.ButtonBase;
using Common.Generated;
using Common.Triggers;
using UI.Common.Button.SceneChange;

namespace UI.Login.Command
{
    public class ToEmailLoginSceneCommand : SceneChangeCommand
    {
        protected override Scene Scene => Scene.EmailLogin;
    }
}