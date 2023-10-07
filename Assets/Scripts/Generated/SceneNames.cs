using UnityEngine;
namespace Generated
{
	public class Scenes
	{
		public string Name { get; }
		private Scenes(string name)
		{
			Name = name;
		}
		public static Scenes UnauthorizedScreen => new Scenes("UnauthorizedScreen");
		public static Scenes LoginScreen => new Scenes("LoginScreen");
		public static Scenes OAuthLoginScreen => new Scenes("OAuthLoginScreen");
		public static Scenes MainMenuScreen => new Scenes("MainMenuScreen");
		public static Scenes PlanetEditingScene => new Scenes("PlanetEditingScene");
		public static Scenes WorldListScreen => new Scenes("WorldListScreen");
		public static Scenes PlanetEditingLoadScene => new Scenes("PlanetEditingLoadScene");
		public static Scenes Initial => new Scenes("Initial");
	}
}
