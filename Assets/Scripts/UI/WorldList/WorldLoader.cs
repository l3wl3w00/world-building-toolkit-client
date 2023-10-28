#nullable enable
using System;
using Client;
using Client.Dto;
using Client.Request;
using Client.Response;
using Common;
using Common.Constants;
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using Generated;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace UI.WorldList
{
    public class WorldLoader : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Transform contentTransform = null!; // asserted in Awake
        [SerializeField] private GameObject uiItemPrefab = null!; // asserted in Awake

        #endregion

        [Inject] private DiContainer _container = null!; // asserted in Awake
        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;

        #region Event Functions

        private void Awake()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey)).ToOption();
            NullChecker.AssertNoneIsNullInType(GetType(),contentTransform, uiItemPrefab, _container);
            var processingStrategy = new WorldListResponseProcessor(contentTransform, uiItemPrefab, _container);
            _client.Value.GetWorlds(processingStrategy).StartCoroutine(this);
        }

        #endregion


        private record WorldListResponseProcessor(
                Transform contentTransform, 
                GameObject uiItemPrefab, DiContainer container) 
            : IResponseProcessStrategy<WorldSummaryDtos>
        {
            public void OnSuccess(WorldSummaryDtos responseDto)
            {
                foreach (var worldSummaryDto in responseDto.value)
                {
                    var initializer = container.InstantiatePrefabForComponent<WorldUiItemInitializer>(uiItemPrefab, contentTransform);
                    initializer.Initialize(worldSummaryDto.Name, worldSummaryDto.Id.ToTypesafe<Planet>());
                }

            }

            public void OnFail(ErrorResponse error)
            {
                
            }
        }
    }
}