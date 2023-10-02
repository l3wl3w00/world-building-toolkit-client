using System;
using Game.Client;
using Game.Client.Dto;
using Game.Constants;
using UnityEngine;

namespace UI.WorldList
{
    public class WorldLoader : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Transform contentTransform;
        public GameObject itemPrefab; // Prefab for UI elements
        public Canvas canvas;

        #endregion

        private WorldBuildingApiClient _client;

        #region Event Functions

        private void Start()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
            StartCoroutine(_client.GetWorlds(CreateItemUI));
        }

        #endregion

        private void CreateItemUI(WorldSummaryDto world)
        {
            var initializer = Instantiate(itemPrefab, contentTransform).GetComponent<WorldUiItemInitializer>();
            initializer.Initialize(world.name, Guid.Parse(world.id));
        }
    }
}