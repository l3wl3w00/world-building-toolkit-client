#nullable enable
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class LineControlPointControl : MonoBehaviour
    {
        #region Serialized Fields

        // [SerializeField] private PlanetMonoBehaviour planetMonoBehaviour = null!; // asserted in Awake
        // [SerializeField] private Camera mainCamera = null!; // asserted in Awake

        #endregion

        #region Event Functions

        private void Awake()
        {
            // NullChecker.AssertNoneIsNullInType(GetType(), planetMonoBehaviour, mainCamera);
        }

        private void Update()
        {
            // var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            //
            // var raycastSuccessful = Physics.Raycast(ray, out var hit);
            // if (!raycastSuccessful) return;
            //
            // var hitControlPoint = hit.collider.gameObject.CompareTag("ControlPoint");
            // if (!hitControlPoint) return;
            //
            // Debug.Log("Mouse Over ControlPoint");
        }

        #endregion
    }
}