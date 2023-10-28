#nullable enable
using Common;
using Common.Utils;
using Generated;
using UnityEngine;

namespace Game.Common
{
    public abstract record PrefabProvider(Prefab Prefab);
    public abstract record CreateParamsFlag;

    public interface IGameObjectFactoryMethod<in TCreateParams, TPrefabProvider, TExactType>
        where TExactType : Component, IGameObjectFactoryMethod<TCreateParams, TPrefabProvider, TExactType>
        where TPrefabProvider : PrefabProvider, new()
        where TCreateParams : CreateParamsFlag
    {
        static TExactType Create(TCreateParams createParams, Transform? transformParent = null)
        {
            var prefab = new TPrefabProvider().Prefab;
            var mono = prefab
                .Instantiate(transformParent.ToOption())
                .GetComponent<TExactType>().ToOption()
                .ExpectNotNull($"Prefab {prefab.Name} does not contain component {nameof(TExactType)}, but is expected to");
            mono.Initialize(createParams);
            return mono;
        }
        void Initialize(TCreateParams createParams);
    }
}