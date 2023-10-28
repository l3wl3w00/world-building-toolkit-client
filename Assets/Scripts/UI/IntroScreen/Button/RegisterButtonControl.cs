#nullable enable
using System;
using Common.Triggers;
using UI.Common.Button;
using UI.IntroScreen.Command;

namespace UI.IntroScreen.Button
{
    public class RegisterButtonControl : ButtonActionTrigger<RegisterCommand>
    {
        private void Update()
        {
            Console.WriteLine("asd");
        }
    }
}