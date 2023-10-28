#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Client.Dto;
using Client.EndpointUtil;
using Client.Response;
using Common;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Client.Request
{
    public sealed record NoResponseBody : JsonSerializable<NoResponseBody>;
    public sealed record NoRequestBody : JsonSerializable<NoRequestBody>;
    
    public interface IWorldBuilderRequest
    {
        public IEnumerator Send();
    }
    public interface IResponseProcessStrategy<in TResponseDto>
        where TResponseDto : JsonSerializable<TResponseDto>
    {
        public void OnSuccess(TResponseDto responseDto);
        public void OnFail(ErrorResponse error);
    }
    
    internal record WorldBuilderRequest<TRequestDto, TResponseDto> : IWorldBuilderRequest
        where TRequestDto : JsonSerializable<TRequestDto>
        where TResponseDto : JsonSerializable<TResponseDto>
    {
        
        private readonly IResponseProcessStrategy<TResponseDto> _responseProcessStrategy;
        private readonly TRequestDto _body;
        private readonly UnityWebRequest _unityWebRequest;

        public WorldBuilderRequest(
            HttpMethod method,
            WorldBuildingApiEndpoint endpoint,
            string token,
            IResponseProcessStrategy<TResponseDto> responseProcessStrategy,
            TRequestDto body)
        {
            var bodyAsString = body.ToJson();
            _responseProcessStrategy = responseProcessStrategy;
            _body = body;
            _unityWebRequest = new UnityWebRequest(endpoint.Value, method.Name());
            if (HasRequestBody)
            {
                _unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyAsString));
            }
            if (HasResponseBody)
            {
                _unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            }
            _unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            _unityWebRequest.SetRequestHeader("Authorization", "Bearer " + token); 
        }

        private bool HasResponseBody => typeof(TResponseDto) != typeof(NoResponseBody);
        private bool HasRequestBody => typeof(TRequestDto) != typeof(NoRequestBody);

        public IEnumerator Send()
        {
            var logText = $"Sending {_unityWebRequest.method} request to {_unityWebRequest.url}";
            if (HasRequestBody)
            {
                logText += $" with body {ToPrettyJson(_body.ToJson())}";
            } 
            Debug.Log(logText);
            yield return _unityWebRequest.SendWebRequest();
            if (_unityWebRequest.result is UnityWebRequest.Result.Success)
            {
                OnSuccess();
            }
            else
            {
                OnFail();
            }
        }

        private void OnFail()
        {
            var downloadHandlerOpt = _unityWebRequest.downloadHandler.ToOption();
            if (_unityWebRequest.result is UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Connection Error");
                return;
            }

            var downloadHandler = downloadHandlerOpt.ExpectNotNull(
                $"Unsuccessful response has no body, but is expected to have body of type {nameof(ErrorResponse)}");

            var error = downloadHandler.text.ToObjectOrError<ErrorResponse>();
            Debug.LogError("Error Response: " + downloadHandler.text);
            error.LogError();
            _responseProcessStrategy.OnFail(error);

        }

        private void OnSuccess()
        {
            var downloadHandlerOpt = _unityWebRequest.downloadHandler.ToOption();
            if (downloadHandlerOpt.NoValue)
            {
                if (HasResponseBody)
                {
                    Debug.LogError($"Successful response has no body, but is expected to have body of type {nameof(TResponseDto)}");
                    return;
                }
                Debug.Log("Successful response has no body, and is expected to have no body");
                var emptyResponse = "{}".ToObjectOrError<TResponseDto>();
                _responseProcessStrategy.OnSuccess(emptyResponse);
                return;
            }

            var responseText = downloadHandlerOpt.Value.text;
            LogResponse(responseText);
            responseText = WrapIntoObjectIfArray(responseText);

            var response = responseText.ToObjectOrError<TResponseDto>();
            _responseProcessStrategy.OnSuccess(response);
        }

        private string WrapIntoObjectIfArray(string responseText)
        {
            var isResponseArray = JToken.Parse(responseText).Type is JTokenType.Array;
            if (isResponseArray)
            {
                responseText = "{ \"value\":" + responseText + "}";
            }

            return responseText;
        }

        private static void LogResponse(string responseText)
        {
            var prettyJsonResponse = ToPrettyJson(responseText);
            Debug.Log("Successfully received JSON response: " + prettyJsonResponse);
        }

        private static string ToPrettyJson(string json)
        {
            return JsonConvert.SerializeObject(
                JsonConvert.DeserializeObject(json),
                Formatting.Indented
            );
        }
    }
}