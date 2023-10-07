#nullable enable
using System;
using Game.Hud.Button;
using Game.Linq;
using UnityEngine;

namespace Game.Hud
{
    public static class HudUtils
    {
        public static void ForEachHudButtonControl(
            this GameObject gameObject, 
            Action<IHudButtonControl> actionOnHudButton) =>
            gameObject.GetComponentsInChildren<IHudButtonControl>().ForEach(actionOnHudButton.Invoke);
    }
}