using Common;
using Common.ButtonBase;
using Common.Triggers;
using InGameUi.Factory;
using Unity.VisualScripting.IonicZip;
using UnityEngine;
using Zenject;

namespace InGameUi.Button
{
    public class CalendarButton : ButtonActionTrigger<OpenOrCloseCalendarListUiCommand>
    {
    }
    
    public class OpenOrCloseCalendarListUiCommand : ActionListener
    {
        [Inject] private CalendarListUiFactory _factory;
        
        private Option<GameObject> _calendarUi = Option<GameObject>.None;
        public override void OnTriggered(NoActionParam param)
        {
            _calendarUi
                .DoIfNull(Open)
                .DoIfNotNull(Close);
        }

        private void Open()
        {
            _calendarUi = _factory.Create().ToOption();
        }

        private void Close(GameObject o)
        {
            _calendarUi = Option<GameObject>.None;
            Destroy(o);
        }
    }
}