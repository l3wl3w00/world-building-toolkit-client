#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using Common.Triggers.GameController;
using InGameUi.Factory;
using Zenject;

namespace InGameUi.Button
{
    public class OpenOrCloseAiAssistantUiButton : ButtonActionTrigger<OpenOrCloseAiAssistantUiCommand>
    {
        
    }

    public class OpenOrCloseAiAssistantUiCommand : ActionListener
    {
        [Inject] private UiController _uiController;
        [Inject] private AiAssistantUiFactory _factory;
        public override void OnTriggered(NoActionParam param)
        {
            _uiController.OpenOrClose(UiType.AiAssistantAsk, () => _factory.Create(_uiController.transform));
        }
    }
}