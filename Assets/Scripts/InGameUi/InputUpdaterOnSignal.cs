#nullable enable
using System.Linq;
using Common.Utils;
using InGameUi.InputFiller;
using UnityEngine;

namespace InGameUi
{
    public class InputUpdaterOnSignal : MonoBehaviour
    { 
        public void UpdateValues()
        {
            FindObjectsOfType<MonoBehaviour>()
                .OfType<IStateObserver>()
                .ForEach(c => c.UpdateValue());
        }
    }
}