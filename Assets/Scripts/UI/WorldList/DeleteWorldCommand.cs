#nullable enable
using Client.Request;
using Client.Response;
using Common;
using Common.ButtonBase;
using Common.Constants;
using Common.Model;
using Common.Model.Abstractions;
using Common.Triggers.GameController;
using UnityEngine;

namespace UI.WorldList
{
    public class DeleteWorldCommand : ActionListenerMono<DeleteWorldCommand.Params>
    {
        private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;
        protected void Start()
        {
            _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey)).ToOption();
        }
        
        public override void OnTriggered(Params param)
        {
            _client
                .DoIfNotNull(c => c
                    .DeleteWorld(param.planetToDelete, new DeleteWorldResponseProcessor(param.uiItemToDelete))
                    .StartCoroutine(this))
                .DoIfNull(() => Debug.LogError($"client was null in {this.name}"));
        }

        private record DeleteWorldResponseProcessor(GameObject GameObjectToDestroy) : IResponseProcessStrategy<NoResponseBody>

        {
            public void OnSuccess(NoResponseBody responseDto)
            {
                Destroy(GameObjectToDestroy);
            }

            public void OnFail(ErrorResponse error)
            {

            }
        }

        public record Params(IdOf<Planet> planetToDelete, GameObject uiItemToDelete) : IActionParam;
    }
}