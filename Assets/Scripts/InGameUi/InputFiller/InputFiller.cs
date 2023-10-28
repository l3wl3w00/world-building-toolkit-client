#nullable enable
using Common;
using Common.Model;
using Common.Model.Query;
using Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Toggle = UnityEngine.UI.Toggle;

namespace InGameUi.InputFiller
{
    public interface IInputFiller
        // : IInjectable<PlanetMonoBehaviour>
    {
        public void UpdateValue();
    }

    public abstract class InputFiller<TComponent, TValue, TQuery> : MonoBehaviour, IInputFiller
        where TComponent : Component
        where TValue : notnull
        where TQuery : IQuery<TValue>
    {
        private TQuery _query = default!; //Asserted in Start
        
        [Inject]
        private void Construct(TQuery query)
        {
            if (typeof(TQuery) == typeof(ModelCollectionQuery<Calendar>))
            {
                
            }

            _query = query;
        }
        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _query);
            OnStart();
        }
        
        protected virtual void OnStart()
        {
            
        }
        
        private Option<TValue> GetValue() => _query.Get().ToOption();

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
        
        protected abstract void SetValue(TComponent component, TValue value);
    }

    public abstract class InputFiller<TComponent, TValue, TQueryParam, TQuery> : MonoBehaviour, IInputFiller
        where TComponent : Component
        where TValue : notnull
        where TQuery : IQuery<TQueryParam,TValue>
    {
        private TQuery _query = default!; //Asserted in Start
        
        [Inject]
        private void Construct(TQuery query)
        {
            if (typeof(TQuery) == typeof(GetCalendarById))
            {
                
            }

            _query = query;
        }
        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _query);
            OnStart();
        }
        
        protected virtual void OnStart()
        {
            
        }

        protected abstract TQueryParam GetQueryParam(TComponent component);
        private Option<TValue> GetValue(TComponent component) => _query.Get(GetQueryParam(component)).ToOption();

        public void UpdateValue()
        {
            GetComponent<TComponent>().ToOption()
                .DoIfNull(() => Debug.LogError($"{GetType().Name} was put on {name}, " +
                                               $"which has no {typeof(TComponent).Name} component", gameObject))
                .DoIfNotNull(SetValueIfNotNull);
            return;

            void SetValueIfNotNull(TComponent component)
            {
                GetValue(component).DoIfNotNull(v =>
                {
                    Debug.Log($"Updating {name}. new value: {v}");
                    SetValue(component, v);
                });
            }
        }
        
        protected abstract void SetValue(TComponent component, TValue value);
    }
    
    public abstract class InputFiller<TComponent, TValue> : MonoBehaviour, IInputFiller
        where TComponent : Component
        where TValue : notnull
    {
        protected void Start()
        {
            OnStart();
        }
        
        protected virtual void OnStart()
        {
            
        }

        protected abstract Option<TValue> GetValue(TComponent component);

        public void UpdateValue()
        {
            GetComponent<TComponent>().ToOption()
                .DoIfNull(() => Debug.LogError($"{GetType().Name} was put on {name}, " +
                                               $"which has no {nameof(TComponent)} component"))
                .DoIfNotNull(SetValueIfNotNull);
            return;

            void SetValueIfNotNull(TComponent component)
            {
                GetValue(component).DoIfNotNull(v =>
                {
                    Debug.Log($"Updating {name}. new value: {v}");
                    SetValue(component, v);
                });
            }
        }
        
        protected abstract void SetValue(TComponent component, TValue value);
    }
    public abstract class GameObjectInputFiller<TValue, TQueryParam, TQuery> : MonoBehaviour, IInputFiller
        where TValue : notnull
        where TQuery : IQuery<TQueryParam,TValue>
    {
        [Inject] private TQuery _query = default!; //Asserted in Start

        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _query);
            OnStart();
        }
        
        protected virtual void OnStart()
        {
            
        }

        protected abstract TQueryParam QueryParam { get; }
        private Option<TValue> GetValue() => _query.Get(QueryParam).ToOption();

        public void UpdateValue()
        {
            GetValue().DoIfNotNull(v =>
            {
                Debug.Log($"Updating {name}. new value: {v}");
                SetValue(gameObject, v);
            });
        }
        
        protected abstract void SetValue(GameObject gameObjectParam, TValue value);
    }
    
    public class TextInputFiller<TQuery> : InputFiller<TMP_InputField, string, TQuery> 
        where TQuery : IQuery<string>
    {
        protected override void SetValue(TMP_InputField component, string value)
        {
            component.text = value;
        }
    }

    public class ToggleInputFiller<TQuery> : InputFiller<Toggle, bool, TQuery>
        where TQuery : IQuery<bool>
    {
        protected override void SetValue(Toggle component, bool value)
        {
            component.isOn = value;
        }
    }
    
    public abstract class ColorPickerInputFiller<TQuery> : InputFiller<FlexibleColorPicker, Color, TQuery>
        where TQuery : IQuery<Color>
    {
        protected override void SetValue(FlexibleColorPicker component, Color value)
        {
            component.SetColorWithNoEvent(value);
        }
    }
    
    public abstract class DropdownInputFiller<TQuery> : InputFiller<Dropdown, int, TQuery>
        where TQuery : IQuery<int>
    {
        protected override void SetValue(Dropdown component, int value)
        {
            component.value = value;
        }
    }
}