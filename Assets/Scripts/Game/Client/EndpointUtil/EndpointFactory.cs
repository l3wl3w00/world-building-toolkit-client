#nullable enable
using System;

namespace Game.Client.EndpointUtil
{
    public class EndpointFactory
    {
        private const string BaseUrl = "https://localhost:44366/";

        public string CreateContinent(Guid planetId)
        {
            return MakeUrl($"planet/{planetId.ToString()}/continent");
        }

        public string CreateWorld()
        {
            return MakeUrl("planet");
        }

        public string GetWorldDetailed(Guid worldId)
        {
            return PlanetWithId(worldId);
        }

        public string DeleteWorld(Guid worldId)
        {
            return PlanetWithId(worldId);
        }

        public string UpdateWorld(Guid worldId)
        {
            return PlanetWithId(worldId);
        }

        public string PatchContinent(Guid continentId)
        {
            return ContinentWithId(continentId);
        }

        public string GetAllWorlds()
        {
            return MakeUrl("planet");
        }


        private string MakeUrl(string path)
        {
            return BaseUrl + path;
        }

        private string PlanetWithId(Guid worldId)
        {
            return MakeUrl($"planet/{worldId.ToString()}");
        }

        private string ContinentWithId(Guid continentId)
        {
            return MakeUrl($"continent/{continentId.ToString()}");
        }
    }
}