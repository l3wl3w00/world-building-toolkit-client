#nullable enable
using System;
using System.Collections.Generic;

namespace Game.Client.Dto
{
    [Serializable]
    public record ContinentDto
    {
        #region Serialized Fields

        public string id = "";
        public string name = "";
        public string description = "";
        public bool inverted;
        public List<PlanetCoordinateDto>? bounds;

        #endregion
    }

    [Serializable]
    public record CreateContinentDto
    {
        #region Serialized Fields

        public string? name;
        public string? description;
        public List<PlanetCoordinateDto>? bounds;

        #endregion
    }

    [Serializable]
    public record PatchContinentDto
    {
        #region Serialized Fields

        public string? name;
        public string? description;
        public bool inverted;

        #endregion

        public PatchContinentDto(bool inverted, string? name = null, string? description = null)
        {
            this.name = name;
            this.description = description;
            this.inverted = inverted;
        }
    }
}