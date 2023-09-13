using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WorldBuilder.Client.Game.Common.Linq;
using WorldBuilder.Client.UI.WorldList;

namespace WorldBuilder.Client.Game.Common.Client
{
    public delegate void ActionOnWorldSummaries(WorldSummaryDto worldSummaryDto);
    public delegate void ActionOnWorldDetailed(WorldDetailedDto worldDetailedDto);
    public class WorldBuildingApiClient
    {
        private string _token;
        private readonly string apiEndpoint = "https://localhost:44366/planet";

        public WorldBuildingApiClient(string token)
        {
            _token = token;
        }

        public IEnumerator GetWorlds(ActionOnWorldSummaries actionOnWorldSummaries)
        {
            yield return SendWebRequest(out var request, apiEndpoint);
            ProcessWorldListResponse(actionOnWorldSummaries, request);
        }

        public IEnumerator GetWorld(Guid id, ActionOnWorldDetailed actionOnWorldDetailed)
        {
            yield return SendWebRequest(out var request, apiEndpoint + "/" + id.ToString());
            ProcessWorldDetailedResponse(actionOnWorldDetailed, request);
        }

        private void ProcessWorldDetailedResponse(ActionOnWorldDetailed actionOnWorldDetailed, UnityWebRequest request)
        {
            if (IsError(request))
            {
                LogError(request);
                return;
            }

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

        private void ProcessWorldListResponse(ActionOnWorldSummaries actionOnWorldSummaries, UnityWebRequest request)
        {
            if (IsError(request))
            {
                LogError(request);
                return;
            }

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

        private UnityWebRequestAsyncOperation SendWebRequest(out UnityWebRequest request, string endpoint)
        {
            request = UnityWebRequest.Get(endpoint);
            request.SetRequestHeader("Authorization", "Bearer " + _token);

            return request.SendWebRequest();
        }
        

        private void LogError(UnityWebRequest request)
        {
            Debug.LogError("Error fetching items: " + request.error);
        }

        private bool IsError(UnityWebRequest request)
        {
            return request.result == UnityWebRequest.Result.ConnectionError ||
                   request.result == UnityWebRequest.Result.ProtocolError;
        }
    }
    
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var newJson = "{ \"array\": " + json + "}";
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private sealed class Wrapper<T>
        {
            public T[] array;
        }
    }
}