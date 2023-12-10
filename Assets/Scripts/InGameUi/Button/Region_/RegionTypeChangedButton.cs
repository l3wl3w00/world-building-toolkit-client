#nullable enable
using System;
using Common.ButtonBase;
using Common.Model;
using Common.Triggers;
using GameController.Commands;
using TMPro;
using Zenject;

namespace InGameUi.Button.Region_
{
    public class RegionTypeChangedButton : UserControlledActionTrigger<TMP_Dropdown>
    {
        [Inject] private ChangeRegionTypeCommand _command;
        protected override void RegisterListener(TMP_Dropdown component)
        {
            component.onValueChanged.AddListener(v =>
            {
                _command.OnTriggered(((RegionType) v).ToActionParam());
            });
        }
    }
}