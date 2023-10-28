#nullable enable
using System;
using System.Collections.Generic;
using Common.Model.Abstractions;
using UnityEngine;

namespace Common.Model
{
    public enum RegionType
    {
        Country, City, Natural, Other
    }

    public partial record Region(
        IdOf<Region> Id,
        IdOf<Continent> ContinentId,
        string Name,
        string Description,
        Color Color,
        RegionType Type,
        bool Inverted,
        List<SphereSurfaceCoordinate> GlobalBounds) : IModel<Region>;
}