#nullable enable
using Game.Planet;
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
        private HudController? _hudController;
        private PlanetControl? _planetControl;

        #region Properties

        public PlanetControl PlanetControl
        {
            set => _planetControl = value;
            protected get
            {
                if (_planetControl == null)
                    Debug.LogError(
                        $"PlanetControl was expected to be initialized in {GetType().Name}, but it was null");
                return _planetControl!;
            }
        }

        public HudController HudController
        {
            set => _hudController = value;
            protected get
            {
                if (_hudController == null)
                    Debug.LogError(
                        $"HudController was expected to be initialized in {GetType().Name}, but it was null");
                return _hudController!;
            }
        }

        #endregion
    }
}