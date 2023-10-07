#nullable enable
using System;
using Game.Planet;
using Game.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hud.Panel
{
    public interface IInputFiller
    {
        PlanetControl PlanetControl { set; }
        public void UpdateValue();
    }

    public abstract class InputFiller<TComponent, TInput> : MonoBehaviour, IInputFiller
        where TComponent : Component
        where TInput : notnull
    {
        private Option<PlanetControl> _planetControl = Option<PlanetControl>.None;

        #region IInputFiller Members

        #region Properties

        public PlanetControl PlanetControl
        {
            protected get => _planetControl.ExpectNotNull(nameof(_planetControl), new Func<PlanetControl>(() => PlanetControl));
            set => _planetControl = value.ToOption();
        }

        #endregion

        public void UpdateValue()
        {
            GetComponent<TComponent>().ToOption()
                .DoIfNull(() => Debug.LogError($"{GetType().Name} was put on {name}, " +
                                               $"which has no {nameof(TComponent)} component"))
                .DoIfNotNull(SetValueIfNotNull);
            return;

            void SetValueIfNotNull(TComponent component)
            {
                GetValue().DoIfNotNull(v =>
                {
                    Debug.Log($"Updating {name}. new value: {v}");
                    SetValue(component, v);
                });
            }
        }

        #endregion

        protected abstract Option<TInput> GetValue();
        protected abstract void SetValue(TComponent component, TInput value);
    }

    public abstract class TextInputFiller : InputFiller<TMP_InputField, string>
    {
        protected override void SetValue(TMP_InputField component, string value)
        {
            component.text = value;
        }
    }

    public abstract class ToggleInputFiller : InputFiller<Toggle, bool>
    {
        protected override void SetValue(Toggle component, bool value)
        {
            component.isOn = value;
        }
    }
}