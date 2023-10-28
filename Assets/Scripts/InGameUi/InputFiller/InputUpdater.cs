using System;
using UnityEngine;

namespace InGameUi.InputFiller
{
    /// <summary>
    /// Updates the IInputFiller components in the children of this GameObject 
    /// </summary>
    public class InputUpdater : MonoBehaviour
    {
        private byte _tick = 0;
        [SerializeField] private int ticksBetweenUpdates = 5;
        private void Update()
        {
            _tick++;
            if (_tick <= ticksBetweenUpdates) return;
            _tick = 0;
            foreach (var inputFiller in GetComponentsInChildren<IInputFiller>())
            {
                inputFiller.UpdateValue();
            }
        }
    }
}