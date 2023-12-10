#nullable enable
using Common.Triggers;
using GameController.Commands;
using UnityEditor.Toolbars;

namespace InGameUi.Button.Region_
{
    public class RegionNameChangedButton : TextInputChangedActionTrigger<ChangeRegionNameCommand> { }
}