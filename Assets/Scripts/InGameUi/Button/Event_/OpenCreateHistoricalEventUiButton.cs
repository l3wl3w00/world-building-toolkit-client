#nullable enable
using System.Linq;
using Common.ButtonBase;
using Common.Model;
using Common.Model.Builder;
using Common.Triggers;
using Game;
using GameController;
using InGameUi.Factory;
using Zenject;

namespace InGameUi.Button.Event_
{
    public class OpenCreateHistoricalEventUiButton : ButtonActionTrigger<OpenCreateHistoricalEventUiCommand>
    {
        
    }

    public class OpenCreateHistoricalEventUiCommand : ActionListener
    {
        [Inject] private UiController _uiController;
        [Inject] private CreateHistoricalEventUiFactory _factory;
        [Inject] private HistoricalEventBuilderHolder _eventBuilderHolder;
        [Inject] private ModelCollection<Calendar> _calendars;
        [Inject] private SignalBus _signalBus;

        public override void OnTriggered(NoActionParam param)
        {
            var builder = _eventBuilderHolder.StartBuildingModel();
            builder.WithDefaultCalendar(_calendars.First().Id);
            _uiController.OpenUi(UiType.CreateHistoricalEvent, _factory.Create());
            _signalBus.Fire<StateChangedSignal>();
        }
    }
}