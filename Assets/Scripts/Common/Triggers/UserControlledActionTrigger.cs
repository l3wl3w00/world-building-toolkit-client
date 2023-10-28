#nullable enable
using UnityEngine;

namespace Common.Triggers
{
    public abstract class UserControlledActionTrigger<TComponent> : MonoBehaviour 
        where TComponent : Component
    {
        protected void Start()
        {
            var component = GetComponent<TComponent>()
                .ToOption()
                .ExpectNotNull($"{GetType().Name} applied to {name}, which has no {typeof(TComponent).Name} Component");
            RegisterListener(component);
            OnStart();
        }

        protected abstract void RegisterListener(TComponent component);

        // private void Start()
        // {
        //     
        //     var button = GetComponent<Button>().ToOption();
        //     var toggle = GetComponent<Toggle>().ToOption();
        //     var colorPicker = GetComponent<FlexibleColorPicker>().ToOption();
        //     
        //     if (button.NoValue && toggle.NoValue && colorPicker.NoValue) 
        //         Debug.LogError($"ButtonControl applied to {name}, which has no Button or Toggle Component");
        //
        //     button.DoIfNotNull(b => b.onClick.AddListener(ButtonListener));
        //     toggle.DoIfNotNull(t => t.onValueChanged.AddListener(ToggleListener));
        //     colorPicker.DoIfNotNull(p => p.onColorChange.AddListener(ColorPickerListener));
        //     OnStart();
        //     return;
        //
        //     void ToggleListener(bool v) => OnClicked(new ToggleButtonParams(v));
        //     void ButtonListener() => OnClicked(new NoActionParams());
        //     void ColorPickerListener(Color color) => OnClicked(new ColorPickerActionParams(color));
        // }
        
        // private void OnClicked(IActionParams actionParams)
        // {
        //     if (actionParams is not TParam param)
        //     {
        //         Debug.LogError($"parameter type {typeof(TParam).Name} is not appropriate for button type {GetType().Name} ");
        //         return;
        //     }
        //
        //     OnClickedTypesafe(param);
        // }

        protected virtual void OnStart()
        {
        }
    }

}