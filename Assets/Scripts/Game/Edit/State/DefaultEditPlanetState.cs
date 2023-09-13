using UnityEngine;
using WorldBuilder.Client.Game.Line;

namespace WorldBuilder.Client.Game.Edit.State
{
    public class DefaultEditPlanetState : IEditPlanetState
    {
        private Planet _planet;

        public DefaultEditPlanetState(Planet planet)
        {
            _planet = planet;
        }

        public void OnClick(Transform selfTransform)
        {
            
        }

        public void OnRotate(Transform selfTransform)
        {
            _planet.RenderUpdatedLines();
        }

        public void OnCancel()
        {
            
        }
    }
}