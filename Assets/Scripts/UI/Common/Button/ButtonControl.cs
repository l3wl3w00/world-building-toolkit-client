#nullable enable
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
            var button = GetComponent<UnityEngine.UI.Button>();
            var toggle = GetComponent<Toggle>();
            if (button is null && toggle is null)
            {
                Debug.LogError($"ButtonControl applied to {name}, which has no Button or Toggle Component");
                return;
            }

            if (button is not null) button.onClick.AddListener(ButtonListener);
            if (toggle is not null) toggle.onValueChanged.AddListener(ToggleListener);
            OnStart();
            return;

            void ToggleListener(bool v)
            {
                OnClicked(new ToggleButtonParams(v));
            }

            void ButtonListener()
            {
                OnClicked(new NoButtonParams());
            }
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