#nullable enable
using Common.ButtonBase;
using Common.Model;
using Common.Model.Abstractions;
using Common.Triggers;
using Common.Triggers.GameController;
using Game;
using Game.Common;
using InGameUi.Factory;
using InGameUi.InputFiller.Event_;
using Zenject;

namespace InGameUi.Button.Event_
{
    public class OpenHistoricalEventDetailedUiButton : UserControlledActionTrigger<UnityEngine.UI.Button>
    {
        [Inject] private OpenHistoricalEventDetailedUiCommand _openHistoricalEventDetailedUiCommand;
        protected override void RegisterListener(UnityEngine.UI.Button component)
        {
            var eventId = gameObject.GetIdOf<HistoricalEvent>();
            component.onClick.AddListener(() => _openHistoricalEventDetailedUiCommand.OnTriggered(eventId.ToActionParam()));
        }
    }

    public class OpenHistoricalEventDetailedUiCommand : ActionListenerMono<SingleActionParam<IdOf<HistoricalEvent>>>
    {
        [Inject] private UiController _uiController;
        [Inject] private HistoricalEventUiDetailedFactory _factory;
        [Inject] private SignalBus _signalBus;
        public override void OnTriggered(SingleActionParam<IdOf<HistoricalEvent>> param)
        {
            _uiController.OpenUi(UiType.SingleHistoricalEventDetailed, _factory.Create(param.Value));
            _signalBus.StateChanged();   
        }
    }
}