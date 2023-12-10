#nullable enable
using System;
using System.Globalization;
using Client.Dto;
using Client.EndpointUtil;
using Client.Response;
using Common;
using Common.ButtonBase;
using Common.Constants;
using Common.Model.Abstractions;
using Common.Triggers.GameController;
using UnityEngine;

namespace Client.Request
{
    public abstract class ApiCallingCommand<TParams, TRequestDto, TResponseDto> :
        ActionListenerMono<TParams>
        where TParams : IActionParam
        where TRequestDto : JsonSerializable<TRequestDto>
        where TResponseDto : JsonSerializable<TResponseDto>
    {
        
        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;

        protected void Start()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey)).ToOption();
            OnStart();
        }

        public sealed override void OnTriggered(TParams param)
        {
            Debug.Log($"Triggered {GetType().FullName}");
            var endpoint = GetEndpoint(new EndpointFactory(), param);
            var responseProcessStrategy = GetResponseProcessStrategy(param);
            BeforeRequestIsSent();
            var requestDto = GetRequestDto(param);
            _client.ExpectNotNull(nameof(_client), (Action<TParams>)OnTriggered)
                .CreateRequestWithBody(Method, endpoint, responseProcessStrategy, requestDto)
                .Send()
                .StartCoroutine(this);
        }

        protected abstract WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, TParams buttonParams);
        protected abstract HttpMethod Method { get; }
        protected abstract TRequestDto GetRequestDto(TParams buttonParams);
        protected abstract IResponseProcessStrategy<TResponseDto> GetResponseProcessStrategy(TParams buttonParams);

        protected virtual void OnStart()
        {
            
        }
        
        protected virtual void BeforeRequestIsSent()
        {
            
        }
    }
    
    public abstract class GetApiCallingCommand<TButtonParams, TResponseDto>
        : ApiCallingCommand<TButtonParams,NoRequestBody,TResponseDto>
        where TButtonParams : IActionParam
        where TResponseDto : JsonSerializable<TResponseDto>
    {
        protected sealed override NoRequestBody GetRequestDto(TButtonParams buttonParams) => new();
        protected sealed override HttpMethod Method => HttpMethod.Get;
    }
    
    public abstract class DelApiCallingCommand<TButtonParams> : ApiCallingCommand<TButtonParams,NoRequestBody,NoResponseBody>
        where TButtonParams : IActionParam 
    {
        protected sealed override NoRequestBody GetRequestDto(TButtonParams buttonParams) => new();
        protected sealed override HttpMethod Method => HttpMethod.Delete;
    }
    
    public abstract class DelApiCallingCommandAndResponseStrategy<TButtonParams> : DelApiCallingCommand<TButtonParams>, IResponseProcessStrategy<NoResponseBody>
        where TButtonParams : IActionParam 
    {
        protected sealed override IResponseProcessStrategy<NoResponseBody> 
            GetResponseProcessStrategy(TButtonParams buttonParams) => this;

        public abstract void OnSuccess(NoResponseBody responseDto);
        public abstract void OnFail(ErrorResponse error);
    }
    
    public abstract class PostApiCallingCommand<TParams, TRequestDto, TResponseDto> : ApiCallingCommand<TParams,TRequestDto,TResponseDto>
        where TParams : IActionParam 
        where TRequestDto : JsonSerializable<TRequestDto>
        where TResponseDto : JsonSerializable<TResponseDto>
    {
        protected sealed override HttpMethod Method => HttpMethod.Post;
    }
    
    public abstract class PatchApiCallingCommand<TParams, TRequestDto, TResponseDto> : ApiCallingCommand<TParams,TRequestDto,TResponseDto>
        where TParams : IActionParam 
        where TRequestDto : JsonSerializable<TRequestDto>
        where TResponseDto : JsonSerializable<TResponseDto>
    {
        protected sealed override HttpMethod Method => HttpMethod.Patch;
    }
    
    public abstract class PutApiCallingCommand<TParams, TRequestDto, TResponseDto> : ApiCallingCommand<TParams,TRequestDto,TResponseDto>
        where TParams : IActionParam 
        where TRequestDto : JsonSerializable<TRequestDto>
        where TResponseDto : JsonSerializable<TResponseDto>
    {
        protected sealed override HttpMethod Method => HttpMethod.Put;
    }
}