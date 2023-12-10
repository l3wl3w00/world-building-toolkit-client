#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Utils;
using Game;
using Game.Common;
using Game.Planet_.Parts;
using GameController;
using InGameUi.Factory;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEngine;
using Zenject;

namespace InGameUi
{
    public enum UiType
    {
        CreateCalendar,
        ListCalendar,
        SingleCalendarDetailed,
        SingleCalendarSummary,
        ListHistoricalEvents,
        CreateHistoricalEvent,
        SingleHistoricalEventDetailed,
        SingleHistoricalEventSummary,
        AiAssistantAsk,
    }
    public class UiController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private PlanetMonoBehaviour _planetMono;

        private float _time = 0;
        
        public void RemoveAllChildren()
        {
            gameObject.GetChildren().ForEach(Destroy);
        }
        
        public void OpenUi(UiType type, GameObject uiGameObject)
        {
            uiGameObject.transform.SetParent(transform);
            uiGameObject
                .GetComponent<CalendarUiTypeHolder>()
                .ToOption()
                .ValueOr(uiGameObject.AddComponent<CalendarUiTypeHolder>())
                .Value = type;
            _signalBus.Fire<StateChangedSignal>();
            UpdatePlanetInteractable();
        }

        private void UpdatePlanetInteractable()
        {
            var noUiIsOpen = NoUiIsOpen;
            Debug.Log($"Updated ReactToUserInput to {noUiIsOpen}");
            _planetMono.ReactToUserInput = noUiIsOpen;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time > 1)
            {
                UpdatePlanetInteractable();
                _time = 0;
            }
        }

        public void CloseUi(UiType type)
        {
            GetCalendarUiTypeHoldersOf(type).ForEach(t => Destroy(t.gameObject));
            _signalBus.Fire<StateChangedSignal>();
            UpdatePlanetInteractable();
        }

        public bool IsOpen(UiType type) => GetCalendarUiTypeHoldersOf(type).Any();

        public void OpenOrClose(UiType type, Func<GameObject> factory)
        {
            if (IsOpen(type))
            {
                CloseUi(type);
            }
            else
            {
                OpenUi(type, factory());
            }
        }

        public bool IsAnyUiOpen => gameObject.GetChildren().Any();
        public bool NoUiIsOpen => !IsAnyUiOpen;


        private IEnumerable<CalendarUiTypeHolder> GetCalendarUiTypeHoldersOf(UiType type) => gameObject
            .GetChildren()
            .Select(g => g.GetComponent<CalendarUiTypeHolder>().ToOption())
            .Where(t => t.HasValue && t.Value.Value == type)
            .Select(t => t.Value);
    }

    public class CalendarUiTypeHolder : ValueHolder<UiType>
    {
        
    }
}