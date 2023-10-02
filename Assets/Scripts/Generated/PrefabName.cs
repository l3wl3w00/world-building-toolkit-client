namespace Generated
{
    public struct Prefab
    {
        public string Name { get; }

        private Prefab(string name)
        {
            Name = name;
        }

        public static Prefab Continent => new("Prefabs/Continent");
        public static Prefab LineControlPoint => new("Prefabs/LineControlPoint");
        public static Prefab MainCamera => new("Prefabs/Main Camera");
        public static Prefab Planet => new("Prefabs/Planet");
        public static Prefab PlanetLine => new("Prefabs/PlanetLine");
        public static Prefab SceneChangeParameters => new("Prefabs/SceneChange/SceneChangeParameters");
        public static Prefab BackGround => new("Prefabs/UI/BackGround");
        public static Prefab CreateContinentPanel => new("Prefabs/UI/Hud/CreateContinentPanel");
        public static Prefab CreatePlanetPanel => new("Prefabs/UI/Hud/CreatePlanetPanel");
        public static Prefab EditContinentPanel => new("Prefabs/UI/Hud/EditContinentPanel");
        public static Prefab EditPlanetPanel => new("Prefabs/UI/Hud/EditPlanetPanel");
        public static Prefab WorldUiItem => new("Prefabs/UI/WorldUiItem");
    }
}