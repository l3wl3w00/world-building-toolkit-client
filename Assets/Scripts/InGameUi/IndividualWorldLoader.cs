#nullable enable
using Client;
using Client.Dto;
using Client.Mapper;
using Client.Request;
using Client.Response;
using Common;
using Common.Constants;
using Common.Model;
using Common.Model.Abstractions;
using Common.Model.Builder;
using Common.SceneChange;
using Common.Utils;
using UnityEngine;
using Zenject;

namespace InGameUi
{
    public class IndividualWorldLoader : MonoBehaviour
    {
        [Inject] private ModelCollection<Calendar> _calendars = new ModelCollection<Calendar>();
        private void Start()
        {
            var worldId = SceneChangeParameters.NonNullInstance(GetType()).GetNonNullable(SceneParamKeys.WorldId);
            var client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
            
            client
                .GetWorld(worldId, new WorldDetailedResponseProcessor())
                .StartCoroutine(this);
        }

        private record WorldDetailedResponseProcessor : IResponseProcessStrategy<WorldDetailedDto>
        {
            public void OnSuccess(WorldDetailedDto responseDto)
            {
                
                new SceneParametersBuilder()
                    .Add(SceneParamKeys.WorldInitializeParams, ToWorldInitParams(responseDto).ToOption())
                    .Save();
                Generated.Scenes.PlanetEditingScene.Load();
            }

            public void OnFail(ErrorResponse error)
            {
                
            }
            
            private PlanetWithRelatedEntities ToWorldInitParams(WorldDetailedDto worldDetailedDto)
            {
                return new PlanetWithRelatedEntities(
                    worldDetailedDto.ToModel(),
                    worldDetailedDto.Continents.ToModels(), 
                    worldDetailedDto.Calendars.ToModels());
            }
        }
    }
}