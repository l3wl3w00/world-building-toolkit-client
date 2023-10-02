#nullable enable
namespace Generated
{
    public class Scenes
    {
        public string Name { get; }
        public static Scenes UnauthorizedScreen => new("UnauthorizedScreen");
        public static Scenes LoginScreen => new("LoginScreen");
        public static Scenes OAuthLoginScreen => new("OAuthLoginScreen");
        public static Scenes MainMenuScreen => new("MainMenuScreen");
        public static Scenes PlanetEditingScene => new("PlanetEditingScene");
        public static Scenes WorldListScreen => new("WorldListScreen");
        public static Scenes PlanetEditingLoadScene => new("PlanetEditingLoadScene");
        public static Scenes Initial => new("Initial");

        private Scenes(string name)
        {
            Name = name;
        }
    }
}