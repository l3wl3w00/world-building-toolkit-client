#nullable enable
using System;
using Game.Planet;
using Game.Util;
using UI.Common.Button;
using UnityEngine;

namespace Game.Hud.Button
{
    public interface IHudButtonControl
    {
        public PlanetControl PlanetControl { set; }
        public HudController HudController { set; }
    }

    public abstract class HudButtonControl<T> : ButtonControl<T>, IHudButtonControl where T : IButtonParams
    {
        private Option<HudController> _hudController = Option<HudController>.None;
        private Option<PlanetControl> _planetControl = Option<PlanetControl>.None;


        #region Properties

        public PlanetControl PlanetControl
        {
            set => _planetControl = value;
            protected get => 
                _planetControl.ExpectNotNull(nameof(_planetControl), new Func<PlanetControl>(() => PlanetControl));
            
        }

        public HudController HudController
        {
            set => _hudController = value;
            protected get => 
                _hudController.ExpectNotNull(nameof(_hudController), new Func<HudController>(() => HudController));
        }

        #endregion
    }
}