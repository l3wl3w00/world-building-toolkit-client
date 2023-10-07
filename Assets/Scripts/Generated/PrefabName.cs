using UnityEngine;
namespace Generated
{
	public struct Prefab
	{
		public string Name { get; }
		private Prefab(string name)
		{
			Name = name;
		}
		public static Prefab Continent => new Prefab("Prefabs/Continent");
		public static Prefab LineControlPoint => new Prefab("Prefabs/LineControlPoint");
		public static Prefab MainCamera => new Prefab("Prefabs/Main Camera");
		public static Prefab Planet => new Prefab("Prefabs/Planet");
		public static Prefab PlanetLine => new Prefab("Prefabs/PlanetLine");
		public static Prefab SceneChangeParameters => new Prefab("Prefabs/SceneChange/SceneChangeParameters");
		public static Prefab BackGround => new Prefab("Prefabs/UI/BackGround");
		public static Prefab CreateBoundedPanel => new Prefab("Prefabs/UI/Hud/CreateBoundedPanel");
		public static Prefab CreatePlanetPanel => new Prefab("Prefabs/UI/Hud/CreatePlanetPanel");
		public static Prefab EditContinentPanel => new Prefab("Prefabs/UI/Hud/EditContinentPanel");
		public static Prefab EditLocationsPanel => new Prefab("Prefabs/UI/Hud/EditLocationsPanel");
		public static Prefab EditPlanetPanel => new Prefab("Prefabs/UI/Hud/EditPlanetPanel");
		public static Prefab WorldUiItem => new Prefab("Prefabs/UI/WorldUiItem");
	}
}
