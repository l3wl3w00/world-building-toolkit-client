#nullable enable
using System.Collections.Generic;
using Common.ButtonBase;
using Common.Model;
using Game.Planet_.Parts.State;
using Zenject;

namespace GameController.Commands
{
    public record CreatePlanetCommandParams(
        Planet planet,
        ICollection<Continent> continents,
        ICollection<Calendar> calendars,
        List<HistoricalEvent> events) : IActionParam;
    public class CreatePlanetCommand : StateDependantCommand<CreatePlanetCommandParams,PlanetInCreationState>
    {
        [Inject] private ModelCollection<Calendar> _calendars;
        protected override Unit Apply(PlanetInCreationState inCreationState, CreatePlanetCommandParams param)
        {
            var initializeParams = new PlanetWithRelatedEntities(param.planet, param.continents, param.calendars, param.events);
            PlanetMono.Planet = param.planet;
            PlanetMono.ToEditPlanetStateInitially(initializeParams);
            _calendars.Add(param.calendars);
            return default;
        }
    }
    
    public class UpdateMeshesCommand : Command
    {
        public override void OnTriggered(NoActionParam param)
        {
            PlanetMono.UpdatePlanetMeshes();
        }
    }
    
    public class UpdatePlanetNameCommand : StateDependantCommand<SingleActionParam<string>,EditPlanetState>
    {

        protected override Unit Apply(EditPlanetState state, SingleActionParam<string> param)
        {
            PlanetMono.Planet = PlanetMono.Planet with { Name = param.Value };
            return default;
        }
    }

}

