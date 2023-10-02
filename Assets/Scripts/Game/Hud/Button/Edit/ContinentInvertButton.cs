#nullable enable
using UI.Common.Button;
using UnityEngine;

namespace Game.Hud.Button.Edit
{
    public class ContinentInvertButton : HudButtonControl<ToggleButtonParams>
    {
        protected override void OnClickedTypesafe(ToggleButtonParams buttonParams)
        {
            PlanetControl.SelectedContinent
                .DoIfNotNull(selectedContinent =>
                {
                    selectedContinent.Invert = buttonParams.Value;
                    selectedContinent.UpdateMesh();
                    HudController.UpdateInputFields();
                })
                .DoIfNull(() => Debug.LogError("Selected continent was null, but tried to invert it"));
        }
    }
}