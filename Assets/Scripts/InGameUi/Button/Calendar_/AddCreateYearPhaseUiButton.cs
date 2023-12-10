#nullable enable
using System;
using Common.ButtonBase;
using Common.Model;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using Game;
using GameController;
using InGameUi.Factory;
using UnityEngine;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class AddCreateYearPhaseUiButton : ButtonActionTrigger<AddCreateYearPhaseUiCommand>
    {
        
    }

    public class AddCreateYearPhaseUiCommand : ActionListenerMono<NoActionParam>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder = null!; // Asserted In Start
        [Inject] private SignalBus _signalBus = null!;
        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(),_calendarBuilderHolder);
        }

        public override void OnTriggered(NoActionParam param)
        {
            _calendarBuilderHolder.Builder.YearPhases.Add(YearPhase.Default());
            _signalBus.Fire<StateChangedSignal>();
        }
    }
}