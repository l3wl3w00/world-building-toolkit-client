#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using Game.Planet_.Parts.State;
using GameController.Commands;
using UnityEngine;
using Zenject;

namespace InGameUi.Button
{
    public class RegionColorChangedButton : ColorActionTrigger<ChangeRegionColorCommand> { }
}