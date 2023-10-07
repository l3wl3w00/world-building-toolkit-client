#nullable enable
using System;
using System.Collections.Generic;
using Game.Util;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Game.Client.Dto
{
    public static class JsonSerializableExtensions
    {
        public static Option<T> ToObject<T>(this string json) where T : JsonSerializable<T> =>
            JsonConvert.DeserializeObject<T>(json).ToOption();

        public static List<T> ToObjectList<T>(this string json) where T : JsonSerializable<T> =>
            JsonConvert.DeserializeObject<List<T>>(json)
                .ToOption()
                .ValueOr(new());

        public static T ToObjectOrError<T>(this string json, string message = "") where T : JsonSerializable<T> =>
            json.ToObject<T>().ExpectNotNull($"Failed to deserialize {nameof(T)} object.\n Json string: \n{json}\n Message: {message}");
    }

    public abstract record JsonSerializable<T> where T : JsonSerializable<T>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void CheckTIsSameAsInherited()
        {
            var tBase = typeof(T).BaseType.ToOption().ExpectNotNull($"{nameof(T)} must have a base type");
            if (typeof(T) != tBase.GenericTypeArguments[0])
            {
                throw new InvalidOperationException("T should be the same as the inheriting class.");
            }
        }
        
        public string ToJson() => JsonConvert.SerializeObject((T) this);
        public static Option<T> FromJson(string json) => JsonConvert.DeserializeObject<T>(json).ToOption();
    }
    
    public record ContinentDto : JsonSerializable<ContinentDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Inverted { get; set; }
        public List<PlanetCoordinateDto> Bounds { get; set; } = new();
    }

    public record CreateContinentDto : JsonSerializable<CreateContinentDto>
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<PlanetCoordinateDto> Bounds { get; set; } = new();
    }

    public record PatchContinentDto : JsonSerializable<PatchContinentDto>
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Inverted { get; set; }
    }
}