#nullable enable
using System;
using System.Collections.Generic;

namespace Game.Client.Dto
{
    [Serializable]
    public record WorldSummaryDto
    {
        #region Serialized Fields

        public string id;
        public string name;

        #endregion
    }

    [Serializable]
    public record CreateWorldDto
    {
        #region Serialized Fields

        public string name;
        public string description;

        #endregion
    }

    [Serializable]
    public record WorldDetailedDto
    {
        #region Serialized Fields

        public string id;
        public string name;
        public string description;
        public List<ContinentDto> continents;

        #endregion
    }
}