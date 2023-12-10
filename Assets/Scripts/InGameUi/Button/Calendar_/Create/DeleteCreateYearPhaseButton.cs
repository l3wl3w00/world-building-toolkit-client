#nullable enable
using System;
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using Game;
using GameController;
using InGameUi.Util;
using UnityEngine;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class DeleteCreateYearPhaseButton : UserControlledActionTrigger<UnityEngine.UI.Button>
    {
        [Inject] private DeleteCreateYearPhaseCommand _command;
        protected override void RegisterListener(UnityEngine.UI.Button component)
        {
            component.onClick.AddListener(() =>
            {
                var index = transform.GetParentDirectlyUnderContent().GetSelfIndexInParent();
                _command.OnTriggered(new(index));
            });
        }
    }

    public class DeleteCreateYearPhaseCommand : ActionListenerMono<SingleActionParam<uint>>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        [Inject] private SignalBus _signalBus = null!;

        public override void OnTriggered(SingleActionParam<uint> index)
        {
            _calendarBuilderHolder.Builder.YearPhases.RemoveAt(index.Value.ToInt());
            _signalBus.Fire<StateChangedSignal>();
        }
    }
}