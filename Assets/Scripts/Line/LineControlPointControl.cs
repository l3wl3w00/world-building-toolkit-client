using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace WorldBuilder.Client
{
    public class LineControlPointControl : MonoBehaviour
    {
        public Camera Camera { private get; set; }
        public PlanetControl PlanetControl;

        void Update()
        {
            var ray = Camera.ScreenPointToRay(Input.mousePosition);
            
            var raycastSuccessful = Physics.Raycast(ray, out var hit);
            if (!raycastSuccessful)  return;
            
            var hitControlPoint = hit.collider.gameObject.CompareTag("ControlPoint");
            if (!hitControlPoint) return;
            
            Debug.Log("Mouse Over ControlPoint");
        }
    }
}
