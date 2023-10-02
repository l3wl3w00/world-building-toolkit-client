using Game.Planet;
using UnityEngine;

namespace Game
{
    public class LineControlPointControl : MonoBehaviour
    {
        #region Serialized Fields

        public PlanetControl planetControl;

        #endregion

        #region Properties

        public Camera Camera { private get; set; }

        #endregion

        #region Event Functions

        private void Update()
        {
            var ray = Camera.ScreenPointToRay(Input.mousePosition);

            var raycastSuccessful = Physics.Raycast(ray, out var hit);
            if (!raycastSuccessful) return;

            var hitControlPoint = hit.collider.gameObject.CompareTag("ControlPoint");
            if (!hitControlPoint) return;

            Debug.Log("Mouse Over ControlPoint");
        }

        #endregion
    }
}