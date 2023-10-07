#nullable enable
using System;
using System.Collections;
using System.Text;
using Game.Client.Dto;
using Game.Client.EndpointUtil;
using Game.Client.Response;
using Game.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Client
{
    public delegate void ActionOnWorldSummaries(WorldSummaryDto worldSummaryDto);

    public delegate void ActionOnWorldDetailed(WorldDetailedDto worldDetailedDto);

    public delegate void ActionOnContinentDto(ContinentDto continentDto);

    public delegate void ActionOnFail(ErrorResponse error);

    public class WorldBuildingApiClient
    {
        private readonly EndpointFactory _endpointFactory = new();
        private readonly string _token;

        public WorldBuildingApiClient(string token)
        {
            _token = token;
        }

        #region Public Api

        public IEnumerator GetWorlds(ActionOnWorldSummaries actionOnWorldSummaries)
        {
            yield return SendGetRequest(out var request, _endpointFactory.GetAllWorlds());
            ProcessWorldListResponse(request, actionOnWorldSummaries);
        }

        public IEnumerator GetWorld(Guid id, ActionOnWorldDetailed actionOnWorldDetailed)
        {
            yield return SendGetRequest(out var request, _endpointFactory.GetWorldDetailed(id));
            ProcessWorldDetailedResponse(request, actionOnWorldDetailed);
        }

        public IEnumerator DeleteWorld(Guid id, Action actionOnSuccess)
        {
            yield return SendDelRequest(out var request, _endpointFactory.DeleteWorld(id));
            if (!LogAndHandleError(request)) actionOnSuccess.Invoke();
        }

        public IEnumerator PatchContinent(
            Guid continentId,
            PatchContinentDto dto,
            ActionOnContinentDto actionOnPatchSuccessful,
            ActionOnFail actionOnFail)
        {
            yield return SendPatchRequest(out var request, dto.ToJson(),
                _endpointFactory.PatchContinent(continentId));
            ProcessContinentPatchResponse(request, actionOnPatchSuccessful, actionOnFail);
        }


        public IEnumerator AddContinent(
            Guid planetId,
            CreateContinentDto dto,
            ActionOnContinentDto actionOnCreated,
            ActionOnFail actionOnFail)
        {
            yield return SendPostRequest(out var request, dto.ToJson(),
                _endpointFactory.CreateContinent(planetId));
            ProcessContinentCreateResponse(request, actionOnCreated, actionOnFail);
        }

        public IEnumerator CreateWorld(
            CreateWorldDto dto,
            ActionOnWorldDetailed actionOnWorldDetailed,
            ActionOnFail actionOnFail)
        {
            yield return SendPostRequest(out var request, dto.ToJson(),
                _endpointFactory.CreateWorld());
            ProcessWorldDetailedResponse(request, actionOnWorldDetailed);
        }

        public IEnumerator UpdateWorld(Guid worldId, CreateWorldDto createWorldDto,
            ActionOnWorldDetailed actionOnUpdated,
            ActionOnFail actionOnFail)
        {
            yield return SendPatchRequest(out var request, createWorldDto.ToJson(),
                _endpointFactory.UpdateWorld(worldId));
            ProcessWorldUpdatedResponse(request, actionOnUpdated, actionOnFail);
        }

        #endregion

        #region Process Response

        private void ProcessWorldUpdatedResponse(UnityWebRequest request, ActionOnWorldDetailed actionOnUpdated,
            ActionOnFail actionOnFail)
        {
            if (LogAndHandleError(request, actionOnFail)) return;

            var requestBody = request.downloadHandler.text;
            var world = WorldDetailedDto
                .FromJson(requestBody)
                .ExpectNotNull($"Cannot deserialize {requestBody} into {nameof(WorldDetailedDto)}");

            actionOnUpdated.Invoke(world);
        }

        private void ProcessContinentPatchResponse(UnityWebRequest request,
            ActionOnContinentDto actionOnPatchSuccessful, ActionOnFail actionOnFail)
        {
            if (LogAndHandleError(request, actionOnFail)) return;

            var requestBody = request.downloadHandler.text;
            var patchedContinent = ContinentDto.FromJson(requestBody)
                .ExpectNotNull($"Cannot deserialize {requestBody} into {nameof(WorldDetailedDto)}");

            actionOnPatchSuccessful.Invoke(patchedContinent);
        }

        private void ProcessContinentCreateResponse(UnityWebRequest request,
            ActionOnContinentDto actionOnContinentDto, ActionOnFail actionOnFail)
        {
            if (LogAndHandleError(request, actionOnFail)) return;

            var createdContinent = request.downloadHandler.text.ToObjectOrError<ContinentDto>();

            actionOnContinentDto.Invoke(createdContinent);
        }

        private void ProcessWorldDetailedResponse(UnityWebRequest request, ActionOnWorldDetailed actionOnWorldDetailed)
        {
            if (LogAndHandleError(request)) return;
            
            var jsonResponse = request.downloadHandler.text;
            Debug.Log("Received JSON: " + jsonResponse);

            var world = jsonResponse.ToObjectOrError<WorldDetailedDto>();
            actionOnWorldDetailed.Invoke(world);
        }

        private void ProcessWorldListResponse(UnityWebRequest request, ActionOnWorldSummaries actionOnWorldSummaries)
        {
            if (LogAndHandleError(request)) return;

            var jsonResponse = request.downloadHandler.text;
            Debug.Log("Received JSON: " + jsonResponse);
            jsonResponse
                .ToObjectList<WorldSummaryDto>()
                .ForEach(actionOnWorldSummaries.Invoke);
        }

        #endregion

        #region Requests

        private UnityWebRequestAsyncOperation SendDelRequest(out UnityWebRequest request, string endpoint)
        {
            Debug.Log($"Sending delete request to {endpoint}");
            request = UnityWebRequest.Delete(endpoint);
            SetAuthHeader(request);

            return request.SendWebRequest();
        }

        private UnityWebRequestAsyncOperation SendPatchRequest(out UnityWebRequest request, string body,
            string endpoint)
        {
            Debug.Log($"Sending patch request to {endpoint}\n with body \n{body}");
            request = CreateRequestWithBody(HttpMethod.Patch, body, endpoint);
            SetAuthHeader(request);

            return request.SendWebRequest();
        }

        private UnityWebRequestAsyncOperation SendGetRequest(out UnityWebRequest request, string endpoint)
        {
            Debug.Log($"Sending get request to {endpoint}");
            request = UnityWebRequest.Get(endpoint);
            SetAuthHeader(request);

            return request.SendWebRequest();
        }

        private UnityWebRequestAsyncOperation SendPostRequest(out UnityWebRequest request, string body, string endpoint)
        {
            Debug.Log($"Sending post request to {endpoint}\n with body \n{body}");
            request = CreateRequestWithBody(HttpMethod.Post, body, endpoint);
            SetAuthHeader(request);

            return request.SendWebRequest();
        }

        #endregion

        #region Handle Errors

        private bool LogAndHandleError(UnityWebRequest request) =>
            LogAndHandleError(request, Option<ActionOnFail>.None);
        
        // returns true if there was an error, and false if there was no error
        private bool LogAndHandleError(UnityWebRequest request, Option<ActionOnFail> actionOnFail)
        {
            var downloadHandler = request.downloadHandler.ToOption();
            
            if (downloadHandler.NoValue || !IsError(request)) return false;
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Connection Error");
                return true;
            }
            
            downloadHandler.Value.text.ToOption()
                .ValueOr("")
                .ToObject<ErrorResponse>()
                .LogErrorIfNull($"Error in response. Response: {downloadHandler.Value.text}. Error: {request.error}")
                .DoIfNotNull(err =>
                {
                    actionOnFail.NullableValue?.Invoke(err);
                    err.LogError();
                });
            
            return true;
        }

        private bool IsError(UnityWebRequest request)
        {
            return request.result == UnityWebRequest.Result.ConnectionError ||
                   request.result == UnityWebRequest.Result.ProtocolError;
        }

        #endregion

        #region Other Utility Methods

        private UnityWebRequest CreateRequestWithBody(HttpMethod method, string body, string endpoint)
        {
            var request = new UnityWebRequest(endpoint, method.Name());
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        private void SetAuthHeader(UnityWebRequest request)
        {
            request.SetRequestHeader("Authorization", "Bearer " + _token);
        }

        #endregion
    }
}