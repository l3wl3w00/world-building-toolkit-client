#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using Game.Planet_.Parts.State;
using GameController;
using GameController.Commands;
using GameController.Queries;
using UnityEngine;

namespace InGameUi.Button
{
    public class ContinentInvertButton : ToggleActionTrigger<InvertSelectedContinentCommand> { }
}