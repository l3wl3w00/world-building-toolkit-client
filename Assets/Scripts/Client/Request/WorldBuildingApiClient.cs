#nullable enable
using System.Collections;
using System.Collections.Generic;
using Client.Dto;
using Client.EndpointUtil;
using Common.Constants;
using Common.Model;
using Common.Model.Abstractions;
using UnityEngine;

namespace Client.Request
{
    // public abstract class WorldBuildingApiRequestFactory<TRequestDto, TResponseDto>
    //     where TRequestDto : JsonSerializable<TRequestDto>
    //     where TResponseDto : JsonSerializable<TResponseDto>
    // {
    //     private const string BaseUrl = "https://localhost:44366/";
    //     protected WorldBuildingApiEndpoint MakeEndpoint(string path) => new(BaseUrl + path);
    //     protected virtual string Token => PlayerPrefs.GetString(AuthConstants.GoogleTokenKey);
    //     protected abstract WorldBuildingApiEndpoint Endpoint { get;}
    //     protected abstract IResponseProcessStrategy<TResponseDto> ResponseProcessStrategy { get;}
    //     protected abstract TRequestDto RequestBody { get;}
    //     protected abstract HttpMethod Method { get;}
    //
    //     public IEnumerator SendRequest()
    //     {
    //         return new WorldBuilderRequest<TRequestDto, TResponseDto>(
    //             Method,
    //             Endpoint,
    //             Token,
    //             ResponseProcessStrategy,
    //             RequestBody)
    //             .Send();
    //     }
    // }
    //
    // public abstract class GetRequest<TResponseDto> 
    //     : WorldBuildingApiRequestFactory<NoRequestBody, TResponseDto>
    //     where TResponseDto : JsonSerializable<TResponseDto>
    // {
    //     protected override NoRequestBody RequestBody => new();
    //     protected override HttpMethod Method => HttpMethod.Get;
    // }
    //
    // public class GetWorldRequest : GetRequest<WorldDetailedDto>
    // {
    //     private readonly IdOf<Planet> _planetId;
    //
    //     public GetWorldRequest(IdOf<Planet> planetId, IResponseProcessStrategy<WorldDetailedDto> responseProcessStrategy)
    //     {
    //         _planetId = planetId;
    //         ResponseProcessStrategy = responseProcessStrategy;
    //     }
    //
    //     protected override WorldBuildingApiEndpoint Endpoint => MakeEndpoint($"planet/{_planetId.Value.ToString()}");
    //     protected override IResponseProcessStrategy<WorldDetailedDto> ResponseProcessStrategy { get; }
    // }

    public class WorldBuildingApiClient
    {
        private readonly EndpointFactory _endpointFactory = new();
        private readonly string _token;

        public WorldBuildingApiClient(string token)
        {
            _token = token;
        }

        public IEnumerator GetWorlds(IResponseProcessStrategy<WorldSummaryDtos> responseProcessStrategy)
        {
            var endpoint = _endpointFactory.GetAllWorlds();
            return CreateGetRequest(endpoint, responseProcessStrategy).Send();
        }

        public IEnumerator GetWorld(IdOf<Planet> id, IResponseProcessStrategy<WorldDetailedDto> responseProcessStrategy)
        {
            var endpoint = _endpointFactory.GetWorldDetailed(id);
            return CreateGetRequest(endpoint, responseProcessStrategy).Send();
        }

        public IEnumerator DeleteWorld(IdOf<Planet> id, IResponseProcessStrategy<NoResponseBody> responseProcessStrategy)
        {
            var endpoint = _endpointFactory.DeleteWorld(id);
            return CreateDelRequest(endpoint, responseProcessStrategy).Send();
        }

        public IEnumerator CreateWorld(
            CreateWorldDto dto,
            IResponseProcessStrategy<WorldDetailedDto> responseProcessStrategy)
        {
            var endpoint = _endpointFactory.CreateWorld();
            return CreateRequestWithBody(HttpMethod.Post, endpoint, responseProcessStrategy, dto).Send();
        }

        public IEnumerator UpdateWorld(
            IdOf<Planet> id,
            PatchWorldDto dto,
            IResponseProcessStrategy<WorldDetailedDto> responseProcessStrategy)
        {
            var endpoint = _endpointFactory.UpdateWorld(id);
            return CreateRequestWithBody(HttpMethod.Post, endpoint, responseProcessStrategy, dto).Send();
        }

        public IEnumerator PatchContinent(
            IdOf<Continent> id,
            PatchContinentDto dto, 
            IResponseProcessStrategy<ContinentDto> responseProcessStrategy)
        {
            var endpoint = _endpointFactory.PatchContinent(id);
            return CreateRequestWithBody(HttpMethod.Patch, endpoint, responseProcessStrategy, dto).Send();
        }


        public IEnumerator AddContinent(
            IdOf<Planet> planetId,
            CreateContinentDto dto,
            IResponseProcessStrategy<ContinentDto> responseProcessStrategy)
        {
            var endpoint = _endpointFactory.CreateContinent(planetId);
            return CreateRequestWithBody(HttpMethod.Post, endpoint, responseProcessStrategy, dto).Send();
        }

        public IEnumerator AddRegion(IdOf<Continent> continentId, CreateRegionDto dto,
            IResponseProcessStrategy<RegionDto> responseProcessStrategy)
        {
            var endpoint = _endpointFactory.AddRegion(continentId);
            return CreateRequestWithBody(HttpMethod.Post, endpoint, responseProcessStrategy, dto).Send();
        }

        public IWorldBuilderRequest CreateRequestWithBody<TRequestDto, TResponseDto>(
            HttpMethod method,
            WorldBuildingApiEndpoint endpoint,
            IResponseProcessStrategy<TResponseDto> responseProcessStrategy,
            TRequestDto body)
            where TRequestDto : JsonSerializable<TRequestDto>
            where TResponseDto : JsonSerializable<TResponseDto>
        {
            return new WorldBuilderRequest<TRequestDto, TResponseDto>(
                method,
                endpoint,
                _token,
                responseProcessStrategy,
                body);
        }

        public IWorldBuilderRequest CreateGetRequest<TResponseDto>(
            WorldBuildingApiEndpoint endpoint,
            IResponseProcessStrategy<TResponseDto> responseProcessStrategy)
            where TResponseDto : JsonSerializable<TResponseDto>
        {
            return new WorldBuilderRequest<NoRequestBody, TResponseDto>(
                HttpMethod.Get,
                endpoint,
                _token,
                responseProcessStrategy,
                new NoRequestBody());
        }

        public IWorldBuilderRequest CreateDelRequest(
            WorldBuildingApiEndpoint endpoint,
            IResponseProcessStrategy<NoResponseBody> responseProcessStrategy)
        {
            return new WorldBuilderRequest<NoRequestBody, NoResponseBody>(
                HttpMethod.Delete,
                endpoint,
                _token,
                responseProcessStrategy,
                new NoRequestBody());
        }
    }
    
    public static class ClientExtensions
    {
        public static void StartCoroutine(this IEnumerator enumerator, MonoBehaviour monoBehaviour)
        {
            monoBehaviour.StartCoroutine(enumerator);
        }
    }
}