#nullable enable
using System;
using Client;
using Common.Model;
using Common.Model.Abstractions;
using Common.Triggers;
using Common.Utils;
using UnityEngine.UI;
using Zenject;

namespace UI.WorldList
{
    public class DeleteWorldButton : UserControlledActionTrigger<Button>
    {
        [Inject] private DeleteWorldCommand _deleteWorldCommand = null!; // Asserted in OnStart
        public IdOf<Planet> Id { private get; set; }

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _deleteWorldCommand);
        }

        protected override void RegisterListener(Button component)
        {
            component.onClick.AddListener(() => _deleteWorldCommand.OnTriggered(new(Id, transform.parent.gameObject)));
        }
    }
}