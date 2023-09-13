using System;

namespace WorldBuilder.Client.Game.Common.Client
{
    [Serializable]
    public record WorldSummaryDto
    {
        public string id;
        public string name;
    }
}