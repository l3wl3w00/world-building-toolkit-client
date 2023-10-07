#nullable enable
using System;
using Game.Client;
using Game.Client.Dto;
using Game.Constants;
using Game.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.WorldList
{
    public class WorldLoader : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Transform contentTransform = null!; // asserted in Awake
        [SerializeField] private GameObject uiItemPrefab = null!; // asserted in Awake

        #endregion

        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;

        #region Event Functions

        private void Awake()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey)).ToOption();
            StartCoroutine(_client.Value.GetWorlds(CreateItemUI));
            NullChecker.AssertNoneIsNullInType(GetType(),contentTransform, uiItemPrefab);
        }

        #endregion

        private void CreateItemUI(WorldSummaryDto world)
        {
            var initializer = Instantiate(uiItemPrefab, contentTransform).GetComponent<WorldUiItemInitializer>();
            initializer.Initialize(world.Name, world.Id);
        }
    }
}