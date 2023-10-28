#nullable enable
using System;
using Client;
using Client.Request;
using Common.Constants;
using Common.Model;
using Common.Model.Abstractions;
using Common.SceneChange;
using Common.Triggers;
using Common.Utils;
using UI.Common.Button;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.WorldList
{
    public class EditWorldButton : UserControlledActionTrigger<Button>
    {
        [Inject] private ToEditWorldScreenCommand _toEditWorldScreen = null!; // Asserted in OnStart
        public IdOf<Planet> Id { private get; set; }

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _toEditWorldScreen);
        }

        protected override void RegisterListener(Button component)
        {
            component.onClick.AddListener(() => _toEditWorldScreen.OnTriggered(new(Id)));
        }
    }
}