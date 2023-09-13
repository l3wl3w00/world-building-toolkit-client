using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WorldBuilder.Client.Game.Common.Client;

namespace WorldBuilder.Client.UI.WorldList
{
    public class WorldLoader : MonoBehaviour
    {
        private readonly string apiEndpoint = "https://localhost:44366/planet";
        private WorldBuildingApiClient client;
        public GameObject itemPrefab;  // Prefab for UI elements
        public Canvas canvas;   

        void Start()
        {
            client = new WorldBuildingApiClient(PlayerPrefs.GetString("google-token"));
            StartCoroutine(client.GetWorlds(CreateItemUI));
        }

        void CreateItemUI(WorldSummaryDto world)
        {
            var newItem = Instantiate(itemPrefab, canvas.transform);
            var button = newItem.GetComponent<Button>();
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = world.name;
        }
    }
}
