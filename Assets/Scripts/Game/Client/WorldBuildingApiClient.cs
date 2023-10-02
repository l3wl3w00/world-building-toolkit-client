using System;
using System.Collections;
using System.Text;
using Game.Client.Dto;
using Game.Client.EndpointUtil;
using Game.Client.Response;
using Game.Linq;
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
            if (!LogAndHandleError(request)) actionOnSuccess?.Invoke();
        }

        public IEnumerator PatchContinent(
            Guid continentId,
            PatchContinentDto dto,
            ActionOnContinentDto actionOnPatchSuccessful,
            ActionOnFail actionOnFail)
        {
            yield return SendPatchRequest(out var request, JsonUtility.ToJson(dto),
                _endpointFactory.PatchContinent(continentId));
            ProcessContinentPatchResponse(request, actionOnPatchSuccessful, actionOnFail);
        }


        public IEnumerator AddContinent(
            Guid planetId,
            CreateContinentDto dto,
            ActionOnContinentDto actionOnCreated,
            ActionOnFail actionOnFail)
        {
            yield return SendPostRequest(out var request, JsonUtility.ToJson(dto),
                _endpointFactory.CreateContinent(planetId));
            ProcessContinentCreateResponse(request, actionOnCreated, actionOnFail);
        }

        public IEnumerator CreateWorld(
            CreateWorldDto dto,
            ActionOnWorldDetailed actionOnWorldDetailed,
            ActionOnFail actionOnFail)
        {
            yield return SendPostRequest(out var request, JsonUtility.ToJson(dto),
                _endpointFactory.CreateWorld());
            ProcessWorldDetailedResponse(request, actionOnWorldDetailed);
        }

        public IEnumerator UpdateWorld(Guid worldId, CreateWorldDto createWorldDto,
            ActionOnWorldDetailed actionOnUpdated,
            ActionOnFail actionOnFail)
        {
            yield return SendPatchRequest(out var request, JsonUtility.ToJson(createWorldDto),
                _endpointFactory.UpdateWorld(worldId));
            ProcessWorldUpdatedResponse(request, actionOnUpdated, actionOnFail);
        }

        #endregion

        #region Process Response

        private void ProcessWorldUpdatedResponse(UnityWebRequest request, ActionOnWorldDetailed actionOnUpdated,
            ActionOnFail actionOnFail)
        {
            if (LogAndHandleError(request, actionOnFail)) return;

            var world = JsonUtility.FromJson<WorldDetailedDto>(request.downloadHandler.text);

            actionOnUpdated.Invoke(world);
        }

        private void ProcessContinentPatchResponse(UnityWebRequest request,
            ActionOnContinentDto actionOnPatchSuccessful, ActionOnFail actionOnFail)
        {
            if (LogAndHandleError(request, actionOnFail)) return;

            var patchedContinent = JsonUtility.FromJson<ContinentDto>(request.downloadHandler.text);

            actionOnPatchSuccessful?.Invoke(patchedContinent);
        }

        private void ProcessContinentCreateResponse(UnityWebRequest request,
            ActionOnContinentDto actionOnContinentDto, ActionOnFail actionOnFail)
        {
            if (LogAndHandleError(request, actionOnFail)) return;

            var createdContinent = JsonUtility.FromJson<ContinentDto>(request.downloadHandler.text);

            actionOnContinentDto?.Invoke(createdContinent);
        }

        private void ProcessWorldDetailedResponse(UnityWebRequest request, ActionOnWorldDetailed actionOnWorldDetailed)
        {
            if (LogAndHandleError(request)) return;

            var jsonResponse = request.downloadHandler.text;
            Debug.Log("Received JSON: " + jsonResponse);

            var world = JsonUtility.FromJson<WorldDetailedDto>(jsonResponse);
            if (world == null)
            {
                Debug.LogError("Deserialized item is null");
                return;
            }

            actionOnWorldDetailed?.Invoke(world);
        }

        private void ProcessWorldListResponse(UnityWebRequest request, ActionOnWorldSummaries actionOnWorldSummaries)
        {
            if (LogAndHandleError(request)) return;

            var jsonResponse = request.downloadHandler.text;
            Debug.Log("Received JSON: " + jsonResponse);

            var worlds = JsonHelper.FromJson<WorldSummaryDto>(jsonResponse);
            if (worlds == null)
            {
                Debug.LogError("Deserialized items are null");
                return;
            }

            worlds.ForEach(w => actionOnWorldSummaries?.Invoke(w));
        }

        #endregion

        #region Requests

        private UnityWebRequestAsyncOperation SendDelRequest(out UnityWebRequest request, string endpoint)
        {
            request = UnityWebRequest.Delete(endpoint);
            SetAuthHeader(request);

            return request.SendWebRequest();
        }

        private UnityWebRequestAsyncOperation SendPatchRequest(out UnityWebRequest request, string body,
            string endpoint)
        {
            request = CreateRequestWithBody(HttpMethod.Patch, body, endpoint);
            SetAuthHeader(request);

            return request.SendWebRequest();
        }

        private UnityWebRequestAsyncOperation SendGetRequest(out UnityWebRequest request, string endpoint)
        {
            request = UnityWebRequest.Get(endpoint);
            SetAuthHeader(request);

            return request.SendWebRequest();
        }

        private UnityWebRequestAsyncOperation SendPostRequest(out UnityWebRequest request, string body, string endpoint)
        {
            request = CreateRequestWithBody(HttpMethod.Post, body, endpoint);
            SetAuthHeader(request);

            return request.SendWebRequest();
        }

        #endregion

        #region Handle Errors

        // returns true if there was an error, and false if there was no error
        private bool LogAndHandleError(UnityWebRequest request, ActionOnFail actionOnFail = null)
        {
            if (request.downloadHandler == null) return false;
            var responseBody = request.downloadHandler.text;

            if (IsError(request))
            {
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError("Connection Error");
                    return true;
                }

                var errorResponse = JsonUtility.FromJson<ErrorResponse>(responseBody);
                if (errorResponse == null)
                {
                    Debug.LogError(responseBody);
                    return true;
                }

                actionOnFail?.Invoke(errorResponse);
                Debug.LogError(request.error);
                Debug.LogError(errorResponse.title);
                return true;
            }

            return false;
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

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var newJson = "{ \"array\": " + json + "}";
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [Serializable]
        private sealed class Wrapper<T>
        {
            #region Serialized Fields

            public T[] array;

            #endregion
        }
    }
}