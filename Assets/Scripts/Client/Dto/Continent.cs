#nullable enable
using System;
using System.Collections.Generic;
using Common;
using Common.Model;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Client.Dto
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

    public abstract record JsonSerializable<TExact> where TExact : JsonSerializable<TExact>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void CheckTIsSameAsInherited()
        {
            var tBase = typeof(TExact).BaseType.ToOption().ExpectNotNull($"{nameof(TExact)} must have a base type");
            if (typeof(TExact) != tBase.GenericTypeArguments[0])
            {
                throw new InvalidOperationException("T should be the same as the inheriting class.");
            }
        }
        
        public string ToJson() => 
            JsonConvert.SerializeObject((TExact) this);
        public static Option<TExact> FromJson(string json) => JsonConvert.DeserializeObject<TExact>(json).ToOption();
    }

    public record ContinentDto(
            Guid Id,
            Guid ParentContinentId,
            string Name,
            string Description,
            bool Inverted,
            ICollection<RegionDto> Regions,
            List<PlanetCoordinateDto> Bounds)
        : JsonSerializable<ContinentDto>;

    public record CreateContinentDto(
            string Name,
            string Description, 
            Guid ParentContinentId,
            List<PlanetCoordinateDto> Bounds)
        : JsonSerializable<CreateContinentDto>;

    public record PatchContinentDto(
            string Name,
            string Description,
            bool Inverted)
        : JsonSerializable<PatchContinentDto>;
}