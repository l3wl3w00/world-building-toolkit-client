#nullable enable
using Common.Generated;
using UnityEngine;
using Zenject;

namespace InGameUi.Factory
{
    public class AiAssistantUiFactory : PlaceholderFactory<Transform, GameObject>
    {
        public class Logic : IFactory<Transform, GameObject>
        {
            [Inject] private DiContainer _container;

            public GameObject Create(Transform parent) =>
                Prefab.AiAssistantUi.Instantiate(_container, parent);
        }
    }
}