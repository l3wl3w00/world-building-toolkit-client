#nullable enable
using System;
using Common.Model;
using Common.Model.Abstractions;
using Common.SceneChange;

namespace Common
{
    public static class SceneParamKeys
    {
        public static SceneParamKey<IdOf<Planet>> WorldId { get; } = new();
        public static SceneParamKey<Option<PlanetWithRelatedEntities>> WorldInitializeParams { get; } = new();
    }
}