#nullable enable
using Common.ButtonBase;
using Common.Model;
using Game.Planet_.Parts.State;
using UnityEngine;

namespace GameController.Commands
{
    public class StartCreatingContinentCommand : Command
    {
        public override void OnTriggered(NoActionParam param)
        {
            PlanetMono.StartCreatingNewContinent();
        }
    }
    
    public class CancelCreatingContinentCommand : StateDependantCommand<ContinentInCreationState>
    {
        protected override Unit Apply(ContinentInCreationState state, NoActionParam param)
        {
            Destroy(state.Builder);
            PlanetMono.ToEditPlanetState();
            return new Unit();
        }
    }

    public class CreateContinentWithParentCommand : StateDependantCommand<SingleActionParam<Continent>,ContinentInCreationState>
    {
        protected override Unit Apply(ContinentInCreationState state, SingleActionParam<Continent> param)
        {
            var continent = param.Value;
            if (continent.IsRoot)
            {
                Debug.LogError("Created Continent contains no parentId in the response!");
                return default;
            }
            state.Builder.Build(continent.ToContinentWithParent());
            state.PlanetMono.UpdatePlanetMeshes();
            state.PlanetMono.ToEditPlanetState();
            Destroy(state.Builder.gameObject);
            return default;
        }
    }
    
    public class InvertSelectedContinentCommand : StateDependantCommand<SingleActionParam<bool>, SelectedContinentState>
    {
        protected override Unit Apply(SelectedContinentState state, SingleActionParam<bool> param)
        {
            var selectedContinent = state.SelectedContinent;
            var selectedContinentModel =
                selectedContinent.Continent;
            selectedContinent.Continent = selectedContinentModel with
            {
                Inverted = param.Value
            };
            PlanetMono.UpdatePlanetMeshes();
            return default;
        }
    }
    
    public class UpdateSelectedContinentModelCommand : StateDependantCommand<SingleActionParam<Continent>, SelectedContinentState>
    {
        protected override Unit Apply(SelectedContinentState state, SingleActionParam<Continent> param)
        {
            state.SelectedContinent.Continent = param.Value;
            return default;
        }
    }
}