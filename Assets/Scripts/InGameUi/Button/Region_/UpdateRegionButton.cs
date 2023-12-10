using System.Linq;
using Client;
using Client.Dto;
using Client.EndpointUtil;
using Client.Mapper;
using Client.Request;
using Client.Response;
using Common;
using Common.ButtonBase;
using Common.Triggers;
using Common.Utils;
using Game;
using GameController.Commands;
using GameController.Queries;
using InGameUi.Util;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace InGameUi.Button.Region_
{
    public class UpdateRegionButton : ButtonActionTrigger<UpdateRegionCommand>
    {
        
    }
    
     public class UpdateRegionCommand : PatchApiCallingCommand<NoActionParam, PatchRegionDto, RegionDto>, IResponseProcessStrategy<RegionDto>
    {
        [Inject] private SelectedRegionQuery _selectedRegionQuery = null!; // Asserted in OnStart
        // [Inject] private UpdateSelectedContinentModelCommand _continentModelCommand = null!; // Asserted in OnStart
        [Inject] private SignalBus _signalBus = null!; // Asserted in OnStart

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _selectedRegionQuery, _signalBus);
        }

        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) =>
            endpointFactory.PatchRegion(_selectedRegionQuery.Get().Id);

        protected override PatchRegionDto GetRequestDto(NoActionParam buttonParams)
        {
            var region = _selectedRegionQuery.Get();
            return new PatchRegionDto(region.Name, region.Description, region.Inverted, region.Type, region.Color.ToDto());
        }

        protected override IResponseProcessStrategy<RegionDto> GetResponseProcessStrategy(NoActionParam buttonParams) 
            => this;

        public void OnSuccess(RegionDto responseDto)
        {
            _signalBus.StateChanged();
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }

}