#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using Game.Planet_.Parts.State;
using GameController.Commands;
using UnityEngine;

namespace InGameUi.Button
{
    public class RegionCreateCancelButton : ButtonActionTrigger<CancelCreatingRegionCommand> { }

}