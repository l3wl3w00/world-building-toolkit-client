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
		public static Prefab ContinentBuilder => new Prefab("Prefabs/ContinentBuilder");
		public static Prefab UISceneContext => new Prefab("Prefabs/Installers/UISceneContext");
		public static Prefab LineControlPoint => new Prefab("Prefabs/LineControlPoint");
		public static Prefab MainCamera => new Prefab("Prefabs/Main Camera");
		public static Prefab Planet => new Prefab("Prefabs/Planet");
		public static Prefab PlanetLine => new Prefab("Prefabs/PlanetLine");
		public static Prefab Region => new Prefab("Prefabs/Region");
		public static Prefab RegionBuilder => new Prefab("Prefabs/RegionBuilder");
		public static Prefab SceneChangeParameters => new Prefab("Prefabs/SceneChange/SceneChangeParameters");
		public static Prefab BackGround => new Prefab("Prefabs/UI/BackGround");
		public static Prefab CalendarCreateUi => new Prefab("Prefabs/UI/Calendar/CalendarCreateUi");
		public static Prefab CalendarListUI => new Prefab("Prefabs/UI/Calendar/CalendarListUI");
		public static Prefab CalendarUIElementDetailed2 => new Prefab("Prefabs/UI/Calendar/CalendarUIElementDetailed2");
		public static Prefab CalendarUIElementSummary => new Prefab("Prefabs/UI/Calendar/CalendarUIElementSummary");
		public static Prefab CalendarUIElementSummary2 => new Prefab("Prefabs/UI/Calendar/CalendarUIElementSummary2");
		public static Prefab YearPhaseUiElement => new Prefab("Prefabs/UI/Calendar/YearPhaseUiElement");
		public static Prefab YearPhaseUiElement2 => new Prefab("Prefabs/UI/Calendar/YearPhaseUiElement2");
		public static Prefab CreateBoundedPanel => new Prefab("Prefabs/UI/Hud/CreateBoundedPanel");
		public static Prefab CreatePlanetPanel => new Prefab("Prefabs/UI/Hud/CreatePlanetPanel");
		public static Prefab CreateRegionPanel => new Prefab("Prefabs/UI/Hud/CreateRegionPanel");
		public static Prefab EditContinentPanel => new Prefab("Prefabs/UI/Hud/EditContinentPanel");
		public static Prefab EditLocationsPanel => new Prefab("Prefabs/UI/Hud/EditLocationsPanel");
		public static Prefab EditPlanetPanel => new Prefab("Prefabs/UI/Hud/EditPlanetPanel");
		public static Prefab EditRegionPanel => new Prefab("Prefabs/UI/Hud/EditRegionPanel");
		public static Prefab WorldUiItem => new Prefab("Prefabs/UI/WorldUiItem");
	}
}
