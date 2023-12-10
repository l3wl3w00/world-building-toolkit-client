using Client.Dto;
using Common.ButtonBase;
using Common.Triggers;
using UI.EmailLoginScreen;
using UnityEngine.UI;
using Zenject;

namespace UI.UserLoginScreen
{
    public class UserLoginButton : UserControlledActionTrigger<Button>
    {
        [Inject] private LoginCommand _loginCommand;
        protected override void RegisterListener(Button component)
        {
            component.onClick.AddListener(() => _loginCommand.OnTriggered(LoginType.ByUsername.ToActionParam()));
        }
    }
}