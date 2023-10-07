#nullable enable
using Game.Util;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common.Button
{
    public abstract class ButtonControl<T> : MonoBehaviour where T : IButtonParams
    {
        public string Name => name;

        #region Event Functions

        private void Start()
        {
            var button = GetComponent<UnityEngine.UI.Button>().ToOption();
            var toggle = GetComponent<Toggle>().ToOption();
            if (button.NoValue && toggle.NoValue) 
                Debug.LogError($"ButtonControl applied to {name}, which has no Button or Toggle Component");

            button.DoIfNotNull(b => b.onClick.AddListener(ButtonListener));
            toggle.DoIfNotNull(t => t.onValueChanged.AddListener(ToggleListener));
            OnStart();
            return;

            void ToggleListener(bool v) => OnClicked(new ToggleButtonParams(v));
            void ButtonListener() => OnClicked(new NoButtonParams());
        }

        #endregion


        private void OnClicked(IButtonParams buttonParams)
        {
            if (buttonParams is not T param)
            {
                Debug.LogError($"parameter type {nameof(T)} is not appropriate for button type {GetType().Name} ");
                return;
            }

            OnClickedTypesafe(param);
        }

        protected abstract void OnClickedTypesafe(T param);

        protected virtual void OnStart()
        {
        }
    }
}