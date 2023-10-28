#nullable enable
using Common.Triggers;
using Game.Planet_.Parts.State;
using GameController;
using GameController.Commands;
using UnityEngine;
using NoActionParam = Common.ButtonBase.NoActionParam;

namespace InGameUi.Button
{
    public class ContinentCreateCancelButton : ButtonActionTrigger<CancelCreatingContinentCommand> { }
}