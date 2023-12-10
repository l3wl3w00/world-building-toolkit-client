#nullable enable
using System;
using System.Text;
using Common.Model;
using Common.Model.Abstractions;

namespace Client.EndpointUtil
{
    public record WorldBuildingApiEndpoint
    {
        private WorldBuildingApiEndpoint(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static UrlBuilder Builder()
        {
            return new UrlBuilder();
        }
        public class UrlBuilder
        {
            private const string BaseUrl = "https://localhost:44366/";
            internal UrlBuilder() {}
            private readonly StringBuilder _url = new StringBuilder();

            public UrlBuilder Planet(IdOf<Planet>? id = null) => AddSegmentWithId("planet", id);
            public UrlBuilder Continent(IdOf<Continent>? id = null) => AddSegmentWithId("continent", id);
            public UrlBuilder Region(IdOf<Region>? id = null) => AddSegmentWithId("region", id);
            public UrlBuilder HistoricalEvent(IdOf<HistoricalEvent>? id = null) => AddSegmentWithId("historicalEvent", id);
            public UrlBuilder Calendar(IdOf<Calendar>? id = null) => AddSegmentWithId("calendar", id);
            public UrlBuilder Calendar(string name) => AddSegment("calendar").AddSegment(name);
            public UrlBuilder Ask() => AddSegment("askAi");
            public UrlBuilder Register() => AddSegment("register");
            public UrlBuilder Login() => AddSegment("login");


            public WorldBuildingApiEndpoint Build()
            {
                _url.Remove(_url.Length - 1, 1);
                return new WorldBuildingApiEndpoint(BaseUrl + _url);
            }

            private UrlBuilder AddSegmentWithId<T>(string segment, IdOf<T>? id)
                where T : IModel<T>
            {
                AddSegment(segment);
                if (id is not null) AddSegment(id.Value.Value.ToString());
                return this;
            }

            private UrlBuilder AddSegment(string segment)
            {
                _url.Append(segment + "/");
                return this;        
            }
        }
    }
    public class EndpointFactory
    {

        public WorldBuildingApiEndpoint CreateContinent(IdOf<Planet> planetId) => UrlBuilder.Planet(planetId).Continent().Build();
        
        public WorldBuildingApiEndpoint Register() => UrlBuilder.Register().Build();
        public WorldBuildingApiEndpoint Login() => UrlBuilder.Login().Build();
        public WorldBuildingApiEndpoint CreateWorld() => UrlBuilder.Planet().Build();
        public WorldBuildingApiEndpoint GetWorldDetailed(IdOf<Planet> worldId) => UrlBuilder.Planet(worldId).Build();
        public WorldBuildingApiEndpoint DeleteWorld(IdOf<Planet> worldId) => UrlBuilder.Planet(worldId).Build();
        public WorldBuildingApiEndpoint DeleteCalendar(IdOf<Planet> worldId, string calendarName) => UrlBuilder.Planet(worldId).Calendar(calendarName).Build();
        public WorldBuildingApiEndpoint UpdateWorld(IdOf<Planet> worldId) => UrlBuilder.Planet(worldId).Build();
        public WorldBuildingApiEndpoint PatchContinent(IdOf<Continent> continentId) => UrlBuilder.Continent(continentId).Build();
        public WorldBuildingApiEndpoint PatchRegion(IdOf<Region> regionId) => UrlBuilder.Region(regionId).Build();
        public WorldBuildingApiEndpoint GetAllWorlds() => UrlBuilder.Planet().Build();
        public WorldBuildingApiEndpoint GetCalendars(IdOf<Planet> planetId) => UrlBuilder.Planet(planetId).Calendar().Build();
        public WorldBuildingApiEndpoint AddCalendar(IdOf<Planet> planetId) => UrlBuilder.Planet(planetId).Calendar().Build();
        public WorldBuildingApiEndpoint AddRegion(IdOf<Continent> continentId) => UrlBuilder.Continent(continentId).Region().Build();
        public WorldBuildingApiEndpoint CreateCalendar(IdOf<Planet> planetId) => UrlBuilder.Planet(planetId).Calendar().Build();
        public WorldBuildingApiEndpoint AskAboutPlanet(IdOf<Planet> planetId) => UrlBuilder.Planet(planetId).Ask().Build();
        public WorldBuildingApiEndpoint AddHistoricalEvent(IdOf<Region> regionId) => UrlBuilder.Region(regionId).HistoricalEvent().Build();

        private WorldBuildingApiEndpoint.UrlBuilder UrlBuilder => WorldBuildingApiEndpoint.Builder();
    }
}