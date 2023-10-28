#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Model.Abstractions;

namespace Common.Model
{
    public partial record Continent(
        IdOf<Continent> Id,
        Option<IdOf<Continent>> ParentIdOpt,
        string Name,
        string Description,
        bool Inverted,
        ICollection<Region> Regions,
        List<SphereSurfaceCoordinate> GlobalBounds) : IModel<Continent>
    {
        public List<SphereSurfaceCoordinate> GlobalBounds { get; } =
            GlobalBounds; // these coordinates will not change when rotating the planet

        private bool IsChildOf(IdOf<Continent> other)
        {
            if (ParentIdOpt.NoValue) return false;
            return ParentIdOpt.Value == other;
        }

        public IEnumerable<Continent> GetChildren(IEnumerable<Continent> continents)
        {
            return continents.Where(c => c.IsChildOf(Id));
        }

        public bool IsRoot => ParentIdOpt.NoValue;

        public ContinentWithParent ToContinentWithParent() => new(
            Id,
            ParentIdOpt
                .ExpectNotNull("Continent was cast to Root continent, but the parent Id was null"),
            Name,
            Description,
            Inverted,
            Regions,
            GlobalBounds);

        public RootContinent ToRootContinent() => new(Id, Name, Description, Inverted, Regions, GlobalBounds);
    }

    public record ContinentWithParent(
        IdOf<Continent> Id,
        IdOf<Continent> ParentId,
        string Name,
        string Description,
        bool Inverted,
        ICollection<Region> Regions,
        List<SphereSurfaceCoordinate> GlobalBounds)
    {

        public Continent ToContinent() => new(Id, ParentId.ToOption(), Name, Description, Inverted, Regions, GlobalBounds);
    }

    public record RootContinent(
        IdOf<Continent> Id,
        string Name,
        string Description,
        bool Inverted,
        ICollection<Region> Regions,
        List<SphereSurfaceCoordinate> GlobalBounds)
    {
        public Continent ToContinent() => new(Id, Option<IdOf<Continent>>.None, Name, Description, Inverted, Regions, GlobalBounds);

    }
}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}